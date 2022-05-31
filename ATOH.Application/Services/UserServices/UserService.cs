using ATOH.Application.Common.Validators;
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
        UserParamsValidator.CheckName(dto.Name);
        UserParamsValidator.CheckBirthday(dto.BirthDay);

        var user = await _userManager.FindByNameAsync(dto.UserName);
        UserParamsValidator.CheckIsRevoked(user.UserName, user.RevokedOn);

        user.Name = dto.Name;
        user.BirthDay = dto.BirthDay;
        user.Gender = dto.Gender;
        user.ModifiedOn = DateTime.Now;
        user.ModifiedBy = modifiedBy;

        return await _userManager.UpdateAsync(user);
    }
}