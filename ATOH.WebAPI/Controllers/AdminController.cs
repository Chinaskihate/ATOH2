using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users.CreateUser;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using ATOH.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("api/Admin")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AdminController(SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IAdminService adminService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _adminService = adminService;
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var createdBy = User.Identity.Name;
        var result = await _adminService.CreateUser(dto, createdBy);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        var userName = User.Identity.Name;
        var result = await _adminService.UpdateUser(dto, userName);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result);
    }

    [HttpGet("CreateAdmin")]
    [AllowAnonymous]
    public async Task<ActionResult> CreateAdmin()
    {
        await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "Admin",
            IsAdmin = true,
            BirthDay = DateTime.Now,
            CreatedBy = "DbInitializer",
            CreatedOn = DateTime.Now,
            Gender = Gender.Other,
            Name = "Admin",
            ModifiedBy = "DbInitializer",
            ModifiedOn = DateTime.Now
        };
        var result = await _userManager.CreateAsync(user, "1234");
        if (user.IsAdmin)
        {
            var createdUser = await _userManager.FindByNameAsync(user.UserName);
            var temp3 = await _userManager.AddToRoleAsync(user, "Admin");
        }

        return Ok(result);
    }
}