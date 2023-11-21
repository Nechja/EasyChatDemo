using System.Dynamic;
using DataAccess.Context;
using DataAccess.Models;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EasyChatApiTest;

public class ChatRepoTests
{
    private readonly DbContextOptions<ChatDbConext> _dbContextOptions;

    public ChatRepoTests()
    {

        _dbContextOptions = new DbContextOptionsBuilder<ChatDbConext>()
            .UseInMemoryDatabase(databaseName: "ChatDb") 
            .Options;
    }

    [Fact]
    public async Task AddUser()
    {
        // Arrange
        var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
        var repo = new ChatRepo(factory);

        // Act
        await repo.AddUser("kayla");

        // Assert
        using (var context = new ChatDbConext(_dbContextOptions))
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == "kayla");
            Assert.NotNull(user);
            Assert.Equal("kayla", user.Name);
        }
    }

    [Fact]
    public async Task AddMessage()
    {
        // Arrange
        var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
        var repo = new ChatRepo(factory);
        await repo.AddUser("a cat");

        // Act
        await repo.AddMessage("meow", "a cat");

        // Assert
        using (var context = new ChatDbConext(_dbContextOptions))
        {
            var message = await context.Messages.Include(m => m.User).FirstOrDefaultAsync(m => m.User.Name == "a cat");
            Assert.NotNull(message);
            Assert.Equal("meow", message.Content);
            Assert.Equal("a cat", message.User.Name);
        }
    }

    [Fact]
    public async Task GetMessages()
    {
        // Arrange
        var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
        var repo = new ChatRepo(factory);

        // Act
        var messages = await repo.GetMessages();

        // Assert
        Assert.NotNull(messages);
        Assert.IsType<List<ExpandoObject>>(messages);
    }

    [Fact]
    public async Task GetMessagesSinceLastActive()
    {
        // Arrange
        var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
        var repo = new ChatRepo(factory);
        await repo.AddUser("kayla");
        await repo.AddUser("a cat");

        using (var setupContext = new ChatDbConext(_dbContextOptions))
        {

            var dummyUser = await setupContext.Users.FirstOrDefaultAsync(u => u.Name == "kayla");
            var aCatUser = await setupContext.Users.FirstOrDefaultAsync(u => u.Name == "a cat");
            // making messages for test with the ecf
            setupContext.Messages.Add(new Message { Content = "hello past", User = dummyUser, TimeStamp = DateTime.UtcNow.AddMinutes(-1) }); // should not be fetched, as it's older than last active. Only new messages should show up
            setupContext.Messages.Add(new Message { Content = "hello future", User = dummyUser, TimeStamp = DateTime.UtcNow.AddMinutes(1) }); //this is a new message to be shown when a user shows up
            await setupContext.SaveChangesAsync();

            dummyUser.LastActive = DateTime.UtcNow.AddSeconds(-30);
            await setupContext.SaveChangesAsync();
        }

        // Act
        var activeUserId = (await new ChatDbConext(_dbContextOptions).Users.FirstAsync(u => u.Name == "kayla")).Id;
        var messages = await repo.GetMessagesSinceLastActive(activeUserId);

        // Assert
        Assert.Single(messages);
        Assert.Equal("hello future", messages.First().Content);
        Assert.Equal("kayla", messages.First().User.Name);
    }

    [Fact]
    public async Task IsUser()
    {
		// Arrange
		var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
		var repo = new ChatRepo(factory);
        await repo.AddUser("kayla");

		// Act
		var isUser = await repo.IsUser("kayla");

		// Assert
		Assert.True(isUser);
	}


    [Fact]
    public async Task GetUser()
    {
        // Arrange
        var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
        var repo = new ChatRepo(factory);
        await repo.AddUser("kayla");

        // Act
        var user = await repo.GetUser("kayla");

        // Assert
        Assert.NotNull(user);
        Assert.Equal("kayla", user.Name);
    }
}