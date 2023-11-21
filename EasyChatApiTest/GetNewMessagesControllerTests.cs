using DataAccess.Context;
using DataAccess.Models;
using DataAccess.Services;
using EasyChatApi.Controllers;
using EasyChatApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;

namespace EasyChatApiTest;

public class GetNewMessagesControllerTests
{
	private readonly DbContextOptions<ChatDbConext> _dbContextOptions;

	public GetNewMessagesControllerTests()
	{
		// Configure the DbContext with InMemory
		_dbContextOptions = new DbContextOptionsBuilder<ChatDbConext>()
			.UseInMemoryDatabase(databaseName: "ChatDbTest") // Unique name for the test instance
			.Options;
	}

	//[Fact]
	//public async Task Get()
	//{
	//	// Arrange
	//	var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
	//	var repo = new ChatRepo(factory);
	//	var User = new User { Name = "kayla"};
	//	await repo.AddUser(User.Name);
	//	await repo.AddMessage("hello world", User.Name);
	//	var controller = new MessagesController(new NullLogger<MessagesController>(), repo);

	//	// Act
	//	var result = await controller.Get(1);

	//	// Assert
	//	var viewResult = Assert.IsType<List<Message>>(result);
	//	var model = Assert.IsAssignableFrom<IEnumerable<Message>>(viewResult);

	//	Assert.NotNull(model);
	//	Assert.True(model.Any(), "hello world");

	//}

	[Fact]
	public async Task Get()
	{
		// Arrange
		var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
		var repo = new ChatRepository(factory);
		var User = new User { Name = "kayla" };
		await repo.AddUser(User.Name);
		await repo.AddMessage("hello world", User.Name);
		var controller = new MessagesController(new NullLogger<MessagesController>(), repo);

		// Act
		var result = await controller.Get();

		// Assert
		var viewResult = Assert.IsType<List<MessageViewModel>>(result);
		var model = Assert.IsAssignableFrom<IEnumerable<MessageViewModel>>(viewResult);

		Assert.NotNull(model);
		Assert.True(model.Any(), "hello world");
	}

	[Fact]
	public async Task Post()
	{
		//uses http to post a new message
		// Arrange

		var factory = new PooledDbContextFactory<ChatDbConext>(_dbContextOptions);
		var repo = new ChatRepository(factory);
		var User = new User { Name = "kayla" };
		await repo.AddUser(User.Name);
		var controller = new MessagesController(new NullLogger<MessagesController>(), repo);

	}


}