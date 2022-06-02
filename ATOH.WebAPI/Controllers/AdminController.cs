using ATOH.Application.Extensions;
using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users.ChangePassword;
using ATOH.Application.Users.CreateUser;
using ATOH.Application.Users.UpdateUser;
using ATOH.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATOH.WebAPI.Controllers;

/// <summary>
/// Controller for admins.
/// </summary>
[ApiController]
[ApiVersionNeutral]
[Route("api/Admin")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="userManager"> UserManager. </param>
    /// <param name="roleManager"> RoleManager. </param>
    /// <param name="adminService"> AdminService. </param>
    public AdminController(UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IAdminService adminService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _adminService = adminService;
    }

    /// <summary>
    /// Create new user.
    /// </summary>
    /// <param name="dto"> CreateUserDto. </param>
    /// <returns> NoContent if user was been created, otherwise bad request with exceptions. </returns>
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
    /// Update user.
    /// </summary>
    /// <param name="dto"> UpdateUserDto. </param>
    /// <returns>Ok if user was been updated, otherwise BadRequest with exceptions. </returns>
    [HttpPost("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        var userName = User.Identity!.Name;
        var result = await _adminService.UpdateUser(dto, userName!);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Create admin(for first admin).
    /// </summary>
    /// <returns> Ok. </returns>
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
        if (result.Succeeded)
        {
            if (user.IsAdmin)
            {
                await _userManager.FindByNameAsync(user.UserName);
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return Ok(user.Id);
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Change password by admin.
    /// </summary>
    /// <param name="dto"> UserName and new password. </param>
    /// <returns> NoContent if password has been updated. </returns>
    [HttpPost("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordByAdminDto dto)
    {
        var result = await _adminService.ChangePassword(dto, User.Identity!.Name!);
        if (result.Succeeded)
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Change UserName by admin.
    /// </summary>
    /// <param name="oldUserName"> Old UserName. </param>
    /// <param name="newUserName"> New UserName. </param>
    /// <returns> Ok with new UserName if it has been changed, otherwise BadRequest. </returns>
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
    /// Get active users.
    /// </summary>
    /// <returns> Active users. </returns>
    [HttpGet("GetActiveUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers()
    {
        return Ok(await _userManager.GetActiveUsers());
    }

    /// <summary>
    /// Get user by UserName.
    /// </summary>
    /// <param name="userName"> UserName. </param>
    /// <returns> UserVm. </returns>
    [HttpGet("GetUser")]
    public async Task<ActionResult<User>> GetUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound(userName);
        }

        return Ok(new
        {
            // TODO: change to user vm with automapper.
            name = user.Name, gender = user.Gender, birthDay = user.BirthDay,
            isActive = user.RevokedOn == null
        });
    }

    /// <summary>
    /// Get users older than some age.
    /// </summary>
    /// <param name="age"> Age in years. </param>
    /// <returns> Users. </returns>
    [HttpGet("GetOlderThan")]
    public async Task<ActionResult<IEnumerable<User>>> GetOlderThan(int age)
    {
        return Ok(await _userManager.GetOlderThan(age));
    }

    /// <summary>
    /// Deletes user.
    /// </summary>
    /// <param name="userName"> UserName. </param>
    /// <param name="isSoft"> Is soft delete. </param>
    /// <returns> Result. </returns>
    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser(string userName, bool isSoft)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound(user);
        }

        if (isSoft)
        {
            user.RevokedOn = DateTime.Now;
            user.RevokedBy = User.Identity!.Name;
            var result = await _userManager.UpdateAsync(user);
            return Ok(result);
        }

        return Ok(await _userManager.DeleteAsync(user));
    }

    /// <summary>
    /// Recovers user.
    /// </summary>
    /// <param name="userName"> UserName. </param>
    /// <returns> Result. </returns>
    [HttpPost("RecoverUser")]
    public async Task<ActionResult> RecoverUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound(user);
        }

        user.RevokedOn = null;
        user.RevokedBy = null;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok("User has been updated.");
        }

        return BadRequest(result);
    }
}