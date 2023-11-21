using DataAccess.Models;

namespace EasyChatApi.ViewModels;

public class MessageViewModel : IMessageModel
{
	public int Id { get; set; }
	public string Content { get; set; }
	public string User { get; set; }
}
