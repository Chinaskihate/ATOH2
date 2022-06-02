using ATOH.Application.Common.Exceptions;
using ATOH.Application.Common.Validators;
using ATOH.Application.Interfaces.UserServices;
using ATOH.Application.Users;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Services.UserServices;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserService(IMapper mapper, UserManager<User> userManager)
    {
        _mapper = mapper;
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

    public async Task<IdentityResult> ChangePassword(ChangePasswordByUserDto dto, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException(userName);
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (result.Succeeded)
        {
            user.ModifiedBy = user.UserName;
            user.ModifiedOn = DateTime.Now;
        }

        return result;
    }

    public async Task<UserLookupDto> GetUserData(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException(userName);
        }

        return _mapper.Map<UserLookupDto>(user);
    }

    public async Task<IdentityResult> ChangeUserName(string oldUserName, string newUserName)
    {
        var user = await _userManager.FindByNameAsync(oldUserName);
        if (user == null)
        {
            throw new UserNotFoundException(oldUserName);
        }

        if (user.RevokedOn != null)
        {
            throw new RevokedUserException(oldUserName, (DateTime)user.RevokedOn);
        }

        var otherUser = await _userManager.FindByNameAsync(newUserName);
        if (otherUser == null)
        {
            throw new UserAlreadyExistsException(newUserName);
        }

        var result = await _userManager.SetUserNameAsync(user, newUserName);
        if (result.Succeeded)
        {
            user.ModifiedBy = user.UserName;
            user.ModifiedOn = DateTime.Now;
        }
        return result;
    }
}