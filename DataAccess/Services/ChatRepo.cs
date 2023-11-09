using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccess.Services;
public class ChatRepo
{
    readonly IDbContextFactory<ChatDbConext> _contextFactory;

    public ChatRepo(IDbContextFactory<ChatDbConext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddUser(string name)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = new User { Name = name };
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsUser(string name)
    {
	    using (var context = _contextFactory.CreateDbContext())
	    {
			return await context.Users.AnyAsync(u => u.Name == name);
		}
	}

    public async Task AddMessage(string content, string userName)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == userName);
            var message = new Message { Content = content, User = user };
            context.Messages.Add(message);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<Message>> GetMessages()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
	        var messages = await context.Messages.Include(m => m.User).ToListAsync();

			return messages;
        }
    }

    public async Task<List<Message>> GetMessagesSinceLastActive(int userId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var responce = await context.Messages.Include(m => m.User).ToListAsync();
            if (user != null)
            {
                responce = responce.Where(m => m.TimeStamp > user.LastActive).ToList();
                user.LastActive = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
            return responce;
        }
    }
}
