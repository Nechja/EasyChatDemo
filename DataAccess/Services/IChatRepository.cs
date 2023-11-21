using DataAccess.Models;

namespace DataAccess.Services;

public interface IChatRepository
{
	Task AddUser(string name);
	Task<bool> IsUser(string name);
	Task AddMessage(string content, string userName);
	Task<IEnumerable<IDictionary<IUserModel, IMessageModel>>> GetMessagesAndUsers();
	Task<IUserModel> GetUser(string name);
}