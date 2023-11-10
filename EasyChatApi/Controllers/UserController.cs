﻿using DataAccess.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyChatApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ChatRepo _repository;

    public UserController(ILogger<UserController> logger, ChatRepo repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpPost(Name = "AddUser")]
    public async Task AddUser(string userName)
    {
        _logger.LogInformation($"Adding User Name: {userName}");
        await _repository.AddUser(userName);
    }

    [HttpGet(Name = "GetUser")]
    public async Task<User> GetUser(string userName)
    {
        _logger.LogInformation($"Getting User Name: {userName}");
        return await _repository.GetUser(userName);
    }
}