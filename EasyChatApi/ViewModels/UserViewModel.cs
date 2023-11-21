using DataAccess.Models;

namespace EasyChatApi.ViewModels;

public class UserViewModel : IUserModel
{
	public int Id { get; set; }
	public string Name { get; set; }
}
