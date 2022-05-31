using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users.CreateUser;
using ATOH.Domain.Models;
using ATOH.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("api/Auth")]
// [Authorize]
public class AuthController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AuthController(SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("Login")]
    // [AllowAnonymous]
    public async Task<ActionResult> Login(LoginViewModel viewModel)
    {
        var user = await _userManager.FindByNameAsync(viewModel.UserName);
        if (user == null)
        {
            return NotFound("Invalid data.");
        }

        var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);
        if (result.Succeeded)
        {
            return Ok("ok");
        }

        return NotFound("Invalid data.");
    }
}