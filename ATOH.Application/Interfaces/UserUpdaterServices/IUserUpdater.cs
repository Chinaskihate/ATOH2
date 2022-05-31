﻿using ATOH.Application.Users.UpdateUser;
using Microsoft.AspNetCore.Identity;

namespace ATOH.Application.Interfaces.UserUpdaterServices;

public interface IUserUpdater
{
    Task<IdentityResult> UpdateUser(UpdateUserDto dto, string modifiedBy);
}