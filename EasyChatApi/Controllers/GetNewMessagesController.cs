using DataAccess.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyChatApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GetNewMessagesController : ControllerBase
{
	private readonly ILogger<GetNewMessagesController> _logger;
	private readonly ChatRepo _repository;

	public GetNewMessagesController(ILogger<GetNewMessagesController> logger, ChatRepo repository)
	{
		_logger = logger;
		_repository = repository;
	}

	[HttpGet(Name = "GetNewMessages")]
	public async Task<IEnumerable<Message>> Get(int userId)
	{
		_logger.LogInformation($"Getting new messages for user {userId}");
		return await _repository.GetMessagesSinceLastActive(userId);
	}
}
