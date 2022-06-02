using ATOH.Application.Interfaces.UserUpdaterServices;
using ATOH.Application.Users;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.CreateUser;
using ATOH.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Interfaces.AdminService;

public interface IAdminService : IUserUpdater
{
    Task<IdentityResult> CreateUser(CreateUserDto dto, string createdBy);

    Task<IdentityResult> ChangePassword(ChangePasswordByAdminDto dto, string modifiedBy);

    Task<IdentityResult> ChangeUserName(string oldUserName, string newUserName, string modifiedBy);

    Task<IEnumerable<User>> GetActiveUsers();

    Task<IEnumerable<User>> GetOlderThan(int age);

    Task<UserLookupDto> GetUserData(string userName);

    Task<IdentityResult> DeleteUser(string userName, bool isSoft, string revokedBy);

    Task<IdentityResult> RecoverUser(string userName, string modifiedBy);
}