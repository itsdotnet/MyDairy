using MyDairy.Domain.Commons;

namespace MyDairy.Domain.Entities;

public class User : Auditable
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
