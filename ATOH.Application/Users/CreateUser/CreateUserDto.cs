using ATOH.Domain.Models;

namespace ATOH.Application.Users.CreateUser;

public class CreateUserDto
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public Gender Gender { get; set; }

    public DateTime BirthDay { get; set; }

    public bool IsAdmin { get; set; }
}