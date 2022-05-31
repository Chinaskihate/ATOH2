﻿using ATOH.Application.Interfaces.UserUpdaterServices;
using ATOH.Application.Users.CreateUser;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Interfaces.AdminService;

public interface IAdminService : IUserUpdater
{
    Task<IdentityResult> CreateUser(CreateUserDto dto, string createdBy);
}