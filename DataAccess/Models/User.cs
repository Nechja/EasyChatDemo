using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public ICollection<Message> Messages { get; set; }
}
