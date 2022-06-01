using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Interfaces.UserServices;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("api/User")]
[Authorize]
public class UserController : Controller
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserService _userService;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IUserService userService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _userService = userService;
    }

    [HttpPost("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        var userName = User.Identity.Name;
        var result = await _userService.UpdateUser(dto, userName);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result);
    }

    [HttpPost("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordByUserDto dto)
    {
        var result = await _userService.ChangePassword(dto, User.Identity.Name);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result);
    }

    [HttpPost("ChangeUserName")]
    public async Task<ActionResult> ChangeUserName(string newUserName)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user.RevokedOn == null)
        {
            return Forbid();
        }
        user.UserName = newUserName;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            user.ModifiedBy = user.UserName;
            user.ModifiedOn = DateTime.Now;
            return Ok(newUserName);
        }

        return BadRequest("Username already exists");
    }

    [HttpPost("GetData")]
    public async Task<ActionResult> GetData()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        
        return Ok(new
        {
            name = user.Name,
            gender = user.Gender,
            birthDay = user.BirthDay,
            isActive = user.RevokedOn == null
        });
    }
}