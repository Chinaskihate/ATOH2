using ATOH.Application.Users.CreateUser;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Interfaces.AdminService;

public interface IAdminService
{
    Task<IdentityResult> CreateUser(CreateUserDto dto, string createdBy);
}