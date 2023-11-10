using DataAccess.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace EasyChatApi.Controllers;


[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly ChatRepo _repository;

    public MessagesController(ILogger<MessagesController> logger, ChatRepo repository)
    {
        _logger = logger;
        _repository = repository;
    }




    [HttpGet(Name = "GetAllMessages")]
    public async Task<List<ExpandoObject>> Get()
    {
        _logger.LogInformation($"Getting all messages");
        dynamic messages = await _repository.GetMessages();
        return messages;
    }

    [HttpPost(Name = "AddMessage")]
    public async Task AddMessage(string content, string userName)
    {
        _logger.LogInformation($"Adding Message: {content} from {userName}");
        await _repository.AddMessage(content, userName);
    }
}

