using ATOH.Domain.Models;

namespace ATOH.Application.Interfaces.DataServices;

public interface IUserDataService
{
    Task<IEnumerable<User>> GetActiveUsers();

    Task<IEnumerable<User>> GetOlderThan(int years);
}