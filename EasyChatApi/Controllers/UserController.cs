using DataAccess.Models;
using DataAccess.Services;
using EasyChatApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyChatApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IChatRepository _repository;

    public UserController(ILogger<UserController> logger, IChatRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpPost(Name = "User")]
    public async Task AddUser(string userName)
    {
        _logger.LogInformation($"Adding User Name: {userName}");
        await _repository.AddUser(userName);
    }

    [HttpGet(Name = "User")]
    public async Task<UserViewModel> GetUser(string userName)
    {
        _logger.LogInformation($"Getting User Name: {userName}");
        return (UserViewModel)await _repository.GetUser(userName);
    }
}