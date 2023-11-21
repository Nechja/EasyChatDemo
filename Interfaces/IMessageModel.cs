namespace DataAccess.Models;

public interface IMessageModel
{
	int Id { get; set; }
	string Content { get; set; }
}