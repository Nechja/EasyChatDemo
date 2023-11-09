using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;

public class Message
{
    [Key]
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
}
