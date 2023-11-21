using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccess.Services;

public class ChatRepository : IChatRepository
{

    readonly ChatDbConext context;

    public ChatRepository(IDbContextFactory<ChatDbConext> contextFactory)
    {
        context = contextFactory.CreateDbContext();
    }

    public async Task AddUser(string name)
    {
        var user = new User { Name = name };
        context.Users.Add(user);
        await context.SaveChangesAsync();

    }

    public async Task<bool> IsUser(string name)
    {

		return await context.Users.AnyAsync(u => u.Name == name);
	}

    public async Task AddMessage(string content, string userName)
    {

        var user = await context.Users.FirstOrDefaultAsync(u => u.Name == userName);
        var message = new Message { Content = content, User = user };
        context.Messages.Add(message);
        await context.SaveChangesAsync();

    }

    public async Task<IEnumerable<IDictionary<IUserModel, IMessageModel>>> GetMessagesAndUsers()
    {

        var messages = await context.Messages.Include(m => m.User).ToListAsync();
        var messageList = new List<Dictionary<IUserModel, IMessageModel>>();
        foreach (var message in messages)
        {
			messageList.Add(new Dictionary<IUserModel, IMessageModel> { { message.User, message } });
		}

		return messageList;

    }

    public async Task<IUserModel> GetUser(string name)  => await context.Users.FirstOrDefaultAsync(u => u.Name == name);


}
