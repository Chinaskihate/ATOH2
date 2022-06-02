using ATOH.Application.Interfaces.UserServices;
using ATOH.Application.Users;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.UpdateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

/// <summary>
///     Controller for users.
/// </summary>
[ApiController]
[ApiVersionNeutral]
[Route("api/User")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="userService"> UserService. </param>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    ///     Updates user.
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

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Changes user password.
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

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Change UserName.
    /// </summary>
    /// <param name="newUserName"> New UserName. </param>
    /// <returns> New UserName, if it has been changed, otherwise errors. </returns>
    [HttpPost("ChangeUserName")]
    public async Task<ActionResult> ChangeUserName(string newUserName)
    {
        var result = await _userService.ChangeUserName(User.Identity!.Name!, newUserName);
        if (result.Succeeded)
        {
            return Ok(newUserName);
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Get user data.
    /// </summary>
    /// <returns> User data. </returns>
    [HttpPost("GetData")]
    public async Task<ActionResult<UserLookupDto>> GetData()
    {
        var dto = await _userService.GetUserData(User.Identity!.Name!);
        return Ok(dto);
    }
}