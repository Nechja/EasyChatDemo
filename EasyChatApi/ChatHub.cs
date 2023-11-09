using DataAccess.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly ChatRepo _repository;

    public ChatHub(ChatRepo repository)
    {
        _repository = repository;
    }

    public async Task SendMessage(string user, string message)
    {
	    if (!await _repository.IsUser(user))
	    {
            await _repository.AddUser(user);
	    }
        await _repository.AddMessage(message, user);
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}