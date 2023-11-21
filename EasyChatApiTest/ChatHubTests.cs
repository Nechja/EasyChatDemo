using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.Context;
using DataAccess.Services;

namespace EasyChatApiTest;
public class ChatHubTests
{
    private readonly IDbContextFactory<ChatDbConext> _contextFactory;

    public ChatHubTests()
    {
        var options = new DbContextOptionsBuilder<ChatDbConext>()
            .UseInMemoryDatabase(databaseName: "InMemoryChatDb")
            .Options;

        var dbContextFactory = new DbContextFactoryMock<ChatDbConext>(options);
        _contextFactory = dbContextFactory;
    }

    [Fact]
    public async Task SendMessage()
    {
        // Arrange
        var mockClients = new Mock<IHubCallerClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

        var chatRepo = new ChatRepository(_contextFactory);
        var chatHub = new ChatHub(chatRepo)
        {
            Clients = mockClients.Object
        };

        // Act
        await chatHub.SendMessage("Kayla", "Hello, World!");

        // Assert
        mockClientProxy.Verify(clientProxy => clientProxy.SendCoreAsync(
            "ReceiveMessage",
            It.Is<object[]>(o => o != null && (string)o[0] == "Kayla" && (string)o[1] == "Hello, World!"),
            default), Times.Once); // checking if the message was sent to the client (pretty sure this is how it works)

        using (var context = _contextFactory.CreateDbContext())
        {
            // checking if it's in the data as well
            var messageCount = await context.Messages.CountAsync();
            Assert.Equal(1, messageCount);

            var message = await context.Messages.FirstOrDefaultAsync(m => m.Content == "Hello, World!");
            Assert.NotNull(message);
            Assert.Equal("Hello, World!", message.Content); 
        }
    }

    private class DbContextFactoryMock<TContext> : IDbContextFactory<TContext> where TContext : DbContext
    {
        // private readonly IServiceProvider _serviceProvider;
        // context

        private readonly DbContextOptions<TContext> _options;

        public DbContextFactoryMock(DbContextOptions<TContext> options)
        {
            _options = options;
        }

        public TContext CreateDbContext()
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), _options);
        }
    }
}

