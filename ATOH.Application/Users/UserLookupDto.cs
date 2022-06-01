using ATOH.Application.Common.Mappings;
using ATOH.Domain.Models;

namespace ATOH.Application.Users;

public class UserLookupDto : IMapWith<User>
{
    public string Name { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateTime BirthDay { get; set; }

    public bool IsActive { get; set; }
}