using ATOH.Application.Interfaces.UserUpdaterServices;
using ATOH.Application.Users;
using ATOH.Application.Users.ChangePassword;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Interfaces.UserServices;

public interface IUserService : IUserUpdater
{
    Task<IdentityResult> ChangePassword(ChangePasswordByUserDto dto, string userName);

    Task<UserLookupDto> GetUser(string userName);

    Task<IdentityResult> ChangeUserName(string oldUserName, string newUserName);
}