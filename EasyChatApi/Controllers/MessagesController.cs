using DataAccess.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using EasyChatApi.ViewModels;

namespace EasyChatApi.Controllers;


[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly ChatRepository _repository;

    public MessagesController(ILogger<MessagesController> logger, ChatRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }




    [HttpGet(Name = "GetAllMessages")]
    public async Task<List<MessageViewModel>> Get()
    {
        _logger.LogInformation($"Getting all messages");
        var messageList = new List<MessageViewModel>();
        foreach (var message in await _repository.GetMessagesAndUsers())
        {
			messageList.Add(new MessageViewModel { User = message.Keys.First().Name, Content = message.Values.First().Content });
			
		}
        return messageList;
    }

    [HttpPost(Name = "AddMessage")]
    public async Task AddMessage(string content, string userName)
    {
        _logger.LogInformation($"Adding Message: {content} from {userName}");
        await _repository.AddMessage(content, userName);
    }
}

