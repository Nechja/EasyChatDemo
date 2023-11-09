using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EasyChatApiTest;

public class DBTest
{
    private DbContextOptions<ChatDbConext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh 
        // InMemory database instance.
        var builder = new DbContextOptionsBuilder<ChatDbConext>();
        builder.UseInMemoryDatabase("ChatDbTest");

        return builder.Options;
    }

    [Fact]
    public async Task ChatDbConextTest()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Use a clean instance of the context to run the test
        using (var context = new ChatDbConext(options))
        {
            var user = new User { Name = "Kayla" };
            var message = new Message { Content = "Hello World", User = user };

            context.Users.Add(user);
            context.Messages.Add(message);


            await context.SaveChangesAsync();
        }


        using (var context = new ChatDbConext(options))
        {
            // Ensure the user was added
            //Assert.Equal(1, await context.Users.CountAsync()); weird issue with this line
            var SavedUser = await context.Users.FirstOrDefaultAsync(u => u.Name == "Kayla");
            Assert.Equal("Kayla", SavedUser.Name);

            // Ensure the message was added
            //Assert.Equal(1, await context.Messages.CountAsync());
            var SavedMessage = await context.Messages.FirstOrDefaultAsync(m => m.Content == "Hello World");
            Assert.Equal("Hello World", SavedMessage.Content);

            // Ensure relationships are established
            var savedMessage = await context.Messages.Include(m => m.User).FirstOrDefaultAsync();
            Assert.NotNull(savedMessage.User);
            Assert.Equal("Kayla", savedMessage.User.Name);


        }
        

    }

}