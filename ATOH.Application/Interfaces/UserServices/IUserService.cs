using ATOH.Application.Users.UpdateUser;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Interfaces.UserServices;

public interface IUserService
{
    Task<IdentityResult> UpdateUser(UpdateUserDto dto, string modifiedBy);
}