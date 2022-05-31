using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATOH.Application.Interfaces.DataServices;
using ATOH.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ATOH.Persistence.DataServices
{
    public class UserDataService : IUserDataService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public UserDataService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<User>> GetActiveUsers()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var users = context.Users.Where(x => x.RevokedOn == null);
                return await users.ToListAsync();
            }
        }

        public async Task<IEnumerable<User>> GetOlderThan(int years)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var users = context.Users.Where(user => (DateTime.Now - user.BirthDay) > TimeSpan.FromDays(365));
                return await users.ToListAsync();
            }
        }
    }
}
