using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Interfaces.UserServices;
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
            return Ok();
        }

        return BadRequest(result);
    }
}