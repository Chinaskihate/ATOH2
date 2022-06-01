using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Interfaces.UserServices;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

/// <summary>
/// Controller for users.
/// </summary>
[ApiController]
[ApiVersionNeutral]
[Route("api/User")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="userManager"> UserManager. </param>
    /// <param name="userService"> UserService. </param>
    public UserController(UserManager<User> userManager,
        IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    /// <summary>
    /// Updates user.
    /// </summary>
    /// <param name="dto"> UpdateUserDto. </param>
    /// <returns> Result. </returns>
    [HttpPost("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        var userName = User.Identity!.Name;
        var result = await _userService.UpdateUser(dto, userName!);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Changes user password.
    /// </summary>
    /// <param name="dto"> Old and new password. </param>
    /// <returns> Result. </returns>
    [HttpPost("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordByUserDto dto)
    {
        var result = await _userService.ChangePassword(dto, User.Identity!.Name!);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Change UserName.
    /// </summary>
    /// <param name="newUserName"> New UserName. </param>
    /// <returns> New UserName, if it has been changed, otherwise errors. </returns>
    [HttpPost("ChangeUserName")]
    public async Task<ActionResult> ChangeUserName(string newUserName)
    {
        var user = await _userManager.FindByNameAsync(User.Identity!.Name);
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

    /// <summary>
    /// Get user data.
    /// </summary>
    /// <returns> User data. </returns>
    [HttpPost("GetData")]
    public async Task<ActionResult> GetData()
    {
        var user = await _userManager.FindByNameAsync(User.Identity!.Name);
        
        return Ok(new
        {
            name = user.Name,
            gender = user.Gender,
            birthDay = user.BirthDay,
            isActive = user.RevokedOn == null
        });
    }
}