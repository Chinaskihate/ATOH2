using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users.CreateUser;
using ATOH.Domain.Models;
using ATOH.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

/// <summary>
/// Controller for authentication.
/// </summary>
[ApiController]
[ApiVersionNeutral]
[Route("api/Auth")]
[Authorize]
public class AuthController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="signInManager"> SignInManager. </param>
    /// <param name="userManager"> UserManager. </param>
    public AuthController(SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Login.
    /// </summary>
    /// <param name="viewModel"> UserName and password. </param>
    /// <returns> Result. </returns>
    [HttpPost("Login")]
    [AllowAnonymous]
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