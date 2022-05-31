using ATOH.Application.Interfaces.DataServices;
using ATOH.Persistence.DataServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ATOH.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IUserDataService, UserDataService>();
        services.AddDbContextFactory<AppDbContext>(options => options.UseSqlite(connectionString));
        return services;
    }
}