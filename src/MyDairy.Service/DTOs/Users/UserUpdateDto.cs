using System.ComponentModel.DataAnnotations;

namespace MyDairy.Service.DTOs.Users;

public class UserUpdateDto
{
    public int Id { get; set; }

    [MaxLength(25)]
    public string Firstname { get; set; }

    [MaxLength(25)]
    public string Lastname { get; set; }

    [MaxLength(25)]
    public string Username { get; set; }
}