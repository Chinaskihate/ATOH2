using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.CreateUser;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

/// <summary>
///     Controller for admins.
/// </summary>
[ApiController]
[ApiVersionNeutral]
[Route("api/Admin")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="userManager"> UserManager. </param>
    /// <param name="roleManager"> RoleManager. </param>
    /// <param name="adminService"> AdminService. </param>
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    ///     Create new user.
    /// </summary>
    /// <param name="dto"> CreateUserDto. </param>
    /// <returns> Nothing or errors. </returns>
    /// <response code="204"> Success. </response>
    /// <response code="400"> If something went wrong. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var createdBy = User.Identity!.Name;
        var result = await _adminService.CreateUser(dto, createdBy!);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Update user.
    /// </summary>
    /// <param name="dto"> UpdateUserDto. </param>
    /// <returns> Nothing or errors. </returns>
    /// <response code="204"> Success. </response>
    /// <response code="400"> If something went wrong. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    /// <response code="404"> If the user not found. </response>
    [HttpPost("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        var userName = User.Identity!.Name;
        var result = await _adminService.UpdateUser(dto, userName!);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Create admin(for first admin).
    /// </summary>
    /// <returns> Nothing or errors. </returns>
    /// <response code="204"> Success. </response>
    /// <response code="400"> If something went wrong. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    [HttpGet("CreateAdmin")]
    [AllowAnonymous]
    public async Task<ActionResult> CreateAdmin()
    {
        var result = await _adminService.CreateFirstAdmin();
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Change password by admin.
    /// </summary>
    /// <param name="dto"> UserName and new password. </param>
    /// <returns> Nothing or errors. </returns>
    /// <response code="204"> Success. </response>
    /// <response code="400"> If something went wrong. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    /// <response code="404"> If the user not found. </response>
    [HttpPost("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordByAdminDto dto)
    {
        var result = await _adminService.ChangePassword(dto, User.Identity!.Name!);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Change UserName by admin.
    /// </summary>
    /// <param name="oldUserName"> Old UserName. </param>
    /// <param name="newUserName"> New UserName. </param>
    /// <returns> New UserName or errors. </returns>
    /// <response code="200"> Success.</response>
    /// <response code="400"> If something went wrong. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    /// <response code="404"> If the user not found. </response>
    /// <response code="409"> If new UserName is already taken. </response>
    [HttpPost("ChangeUserName")]
    public async Task<ActionResult> ChangeUserName(string oldUserName, string newUserName)
    {
        var result = await _adminService.ChangeUserName(oldUserName, newUserName, User.Identity!.Name!);
        if (result.Succeeded)
        {
            return Ok(newUserName);
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Get active users.
    /// </summary>
    /// <returns> Active users. </returns>
    /// <response code="200"> Success. </response>
    /// <response code="401"> If the user if unauthorized.</response>
    [HttpGet("GetActiveUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers()
    {
        return Ok(await _adminService.GetActiveUsers());
    }

    /// <summary>
    ///     Get user by UserName.
    /// </summary>
    /// <param name="userName"> UserName. </param>
    /// <returns> UserVm. </returns>
    /// <response code="200"> Success. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    /// <response code="404"> If the user not found. </response>
    [HttpGet("GetUserData")]
    public async Task<ActionResult<User>> GetUserData(string userName)
    {
        return Ok(await _adminService.GetUserData(userName));
    }

    /// <summary>
    ///     Get users older than some age.
    /// </summary>
    /// <param name="age"> Age in years. </param>
    /// <returns> Users. </returns>
    /// <response code="200"> Success. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    [HttpGet("GetOlderThan")]
    public async Task<ActionResult<IEnumerable<User>>> GetOlderThan(int age)
    {
        return Ok(await _adminService.GetOlderThan(age));
    }

    /// <summary>
    ///     Deletes user.
    /// </summary>
    /// <param name="userName"> UserName. </param>
    /// <param name="isSoft"> Is soft delete. </param>
    /// <returns> Nothing or errors. </returns>
    /// <response code="204"> Success. </response>
    /// <response code="400"> If something went wrong. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    /// <response code="404"> If user not found. </response>
    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser(string userName, bool isSoft)
    {
        var result = await _adminService.DeleteUser(userName, isSoft, User.Identity!.Name!);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    ///     Recovers user.
    /// </summary>
    /// <param name="userName"> UserName. </param>
    /// <returns> Nothing or errors. </returns>
    /// <response code="204"> Success.</response>
    /// <response code="400"> If something went wrong. </response>
    /// <response code="401"> If the user if unauthorized. </response>
    /// <response code="404"> If the user not found. </response>
    [HttpPost("RecoverUser")]
    public async Task<ActionResult> RecoverUser(string userName)
    {
        var result = await _adminService.RecoverUser(userName, User.Identity!.Name!);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }
}