using ATOH.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ATOH.Persistence;

public class DbInitializer
{
    public static void Initialize(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        using (var dbContext = dbContextFactory.CreateDbContext())
        {
            if (dbContext.Database.EnsureCreated())
            {
                //dbContext.Users.Add(new User
                //{
                //    Id = Guid.NewGuid(),
                //    UserName = "Admin",
                //    PasswordHash = "AQAAAAEAACcQAAAAEGKJhvJTklwjePLXVRhOMbiwKC3NfzmFuPASgXTJGia4FT1U5dDr4j7qTVdJ4MNB",
                //    IsAdmin = true,
                //    BirthDay = DateTime.Now,
                //    CreatedBy = "DbInitializer",
                //    CreatedOn = DateTime.Now,
                //    Gender = Gender.Other,
                //    Name = "Admin",
                //    ModifiedBy = "DbInitializer",
                //    ModifiedOn = DateTime.Now
                //});
                //dbContext.SaveChanges();
            }
        }
    }
}