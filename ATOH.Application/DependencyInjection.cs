using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Services.AdminService;
using Microsoft.Extensions.DependencyInjection;

namespace ATOH.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAdminService, AdminService>();

        return services;
    }
}