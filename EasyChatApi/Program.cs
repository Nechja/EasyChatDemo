using DataAccess.Context;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<ChatDbConext>(options => options.UseInMemoryDatabase("ChatDb"));
builder.Services.AddScoped<ChatRepository>();
builder.Services.AddSignalR();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
else
{

}



//app.UseCors("AllowBlazorOrigin");

app.UseHttpsRedirection();

app.UseRouting();

//app.UseBlazorFrameworkFiles();

//app.MapFallbackToFile("index.html");

app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
