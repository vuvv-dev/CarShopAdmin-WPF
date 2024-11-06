using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities;

public sealed class User
{
    [Key]
    public int Id { get; set; }
    public string FullName { get; set; } = null;
    public string Username { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
}
