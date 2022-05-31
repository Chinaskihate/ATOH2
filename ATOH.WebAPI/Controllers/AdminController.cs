﻿using ATOH.Application.Extensions;
using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Users.ChangePassword;
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

    [HttpPost("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordByAdminDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        await _userManager.RemovePasswordAsync(user);
        var result = await _userManager.AddPasswordAsync(user, dto.NewPassword);
        if (result.Succeeded)
        {
            user.ModifiedBy = User.Identity.Name;
            user.ModifiedOn = DateTime.Now;
            return Ok();
        }

        return BadRequest();
    }

    [HttpPost("ChangeUserName")]
    public async Task<ActionResult> ChangeUserName(string oldUserName, string newUserName)
    {
        var user = await _userManager.FindByNameAsync(oldUserName);
        user.UserName = newUserName;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            user.ModifiedBy = User.Identity.Name;
            user.ModifiedOn = DateTime.Now;
            return Ok(newUserName);
        }

        return BadRequest("Username already exists");
    }

    [HttpGet("GetActiveUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers()
    {
        return Ok(await _userManager.GetActiveUsers());
    }

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
            name = user.Name, gender = user.Gender, birthDay = user.BirthDay,
            isActive = user.RevokedOn == null
        });
    }

    [HttpGet("GetOlderThan")]
    public async Task<ActionResult<IEnumerable<User>>> GetOlderThan(int years)
    {
        return Ok(await _userManager.GetOlderThan(years));
    }
}