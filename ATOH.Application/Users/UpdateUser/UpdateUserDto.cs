using ATOH.Domain.Models;

namespace ATOH.Application.Users.UpdateUser;

public class UpdateUserDto
{
    public string UserName { get; set; }

    public string Name { get; set; }

    public Gender Gender { get; set; }

    public DateTime BirthDay { get; set; }
}