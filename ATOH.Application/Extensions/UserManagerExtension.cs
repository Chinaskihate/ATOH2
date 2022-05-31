using ATOH.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ATOH.Application.Extensions;

public static class UserManagerExtension
{
    public static async Task<IEnumerable<User>> GetActiveUsers(this UserManager<User> userManager)
    {
        return await userManager.Users.Where(u => u.RevokedOn == null)
            .OrderBy(u => u.CreatedOn)
            .ToListAsync();
    }

    public static async Task<IEnumerable<User>> GetOlderThan(this UserManager<User> userManager, int years)
    {
        return (await userManager.Users
            .ToListAsync())
            .Where(u => (DateTime.Now - u.BirthDay).Value.Days > years * 365)
            .OrderBy(u => u.BirthDay);
    }
}