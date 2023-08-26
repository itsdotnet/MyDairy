using MyDairy.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyDairy.Service.DTOs.Users;

public class UserCreationDto
{
    [MaxLength(25)]
    public string Firstname { get; set; }

    [MaxLength(25)]
    public string Lastname { get; set; }

    [MaxLength(25)]
    public string Username { get; set; }

    public string Password { get; set; }
}