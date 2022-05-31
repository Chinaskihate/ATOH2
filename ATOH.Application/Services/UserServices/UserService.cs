using ATOH.Application.Extensions;
using ATOH.Application.Interfaces.UserServices;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Services.UserServices;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> UpdateUser(UpdateUserDto dto, string modifiedBy)
    {
        if (string.IsNullOrEmpty(dto.Name))
        {
            return IdentityResult.Failed();
        }

        if (dto.BirthDay > DateTime.Now)
        {
            return IdentityResult.Failed();
        }

        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user.RevokedOn != null)
        {
            return IdentityResult.Failed();
        }

        user.Name = dto.Name;
        user.BirthDay = dto.BirthDay;
        user.Gender = dto.Gender;
        user.ModifiedOn = DateTime.Now;
        user.ModifiedBy = modifiedBy;

        return await _userManager.UpdateAsync(user);
    }
}