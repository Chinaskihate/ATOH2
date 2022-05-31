using ATOH.Application.Interfaces.AdminService;
using ATOH.Application.Interfaces.UserServices;
using ATOH.Application.Services.AdminServices;
using ATOH.Application.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;

namespace ATOH.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}