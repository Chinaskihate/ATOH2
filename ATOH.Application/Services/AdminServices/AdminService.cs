using ATOH.Application.Common.Exceptions;
using ATOH.Application.Common.Validators;
using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.CreateUser;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ATOH.Application.Services.AdminServices;

public class AdminService : IAdminService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public AdminService(UserManager<User> userManager, IMapper _mapper)
    {
        _userManager = userManager;
        this._mapper = _mapper;
    }

    public async Task<IdentityResult> CreateUser(CreateUserDto dto, string createdBy)
    {
        UserParamsValidator.CheckName(dto.Name);
        UserParamsValidator.CheckBirthday(dto.BirthDay);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName,
            IsAdmin = dto.IsAdmin,
            BirthDay = dto.BirthDay,
            CreatedBy = createdBy,
            CreatedOn = DateTime.Now,
            Gender = dto.Gender,
            Name = dto.Name,
            ModifiedBy = createdBy,
            ModifiedOn = DateTime.Now
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (user.IsAdmin)
        {
            var createdUser = await _userManager.FindByNameAsync(user.UserName);
            await _userManager.AddToRoleAsync(createdUser, "Admin");
        }

        return result;
    }

    public async Task<IdentityResult> ChangePassword(ChangePasswordByAdminDto dto, string modifiedBy)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        await _userManager.RemovePasswordAsync(user);
        var result = await _userManager.AddPasswordAsync(user, dto.NewPassword);

        if (result.Succeeded)
        {
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.Now;
        }

        return result;
    }

    public async Task<IdentityResult> UpdateUser(UpdateUserDto dto, string modifiedBy)
    {
        UserParamsValidator.CheckName(dto.Name);
        UserParamsValidator.CheckBirthday(dto.BirthDay);

        var user = await _userManager.FindByNameAsync(dto.UserName);

        user.Name = dto.Name;
        user.BirthDay = dto.BirthDay;
        user.Gender = dto.Gender;
        user.ModifiedOn = DateTime.Now;
        user.ModifiedBy = modifiedBy;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ChangeUserName(string oldUserName, string newUserName, string modifiedBy)
    {
        var user = await _userManager.FindByNameAsync(oldUserName);
        if (user == null)
        {
            throw new UserNotFoundException(oldUserName);
        }

        var result = await _userManager.SetUserNameAsync(user, newUserName);
        if (result.Succeeded)
        {
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.Now;
        }

        return result;
    }

    public async Task<IEnumerable<User>> GetActiveUsers()
    {
        return await _userManager.Users.Where(u => u.RevokedOn == null)
            .OrderBy(u => u.CreatedOn)
            .ToListAsync();
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
}