using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users.CreateUser;
using ATOH.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Services.AdminServices;

public class AdminService : IAdminService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public AdminService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateUser(CreateUserDto dto, string createdBy)
    {
        var user = new User()
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
}