using System.Reflection;
using System.Text;
using ATOH.Application;
using ATOH.Application.Common.Mappings;
using ATOH.Domain.Models;
using ATOH.Persistence;
using ATOH.WebAPI;
using ATOH.WebAPI.Middleware;
using ATOH.WebAPI.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services, builder.Configuration);
var app = builder.Build();
Configure(app, app.Environment);

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var factory = serviceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        DbInitializer.Initialize(factory);
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Error in db creating! {ex.Message}");
        return;
    }
}

app.Run();

void RegisterServices(IServiceCollection services, IConfiguration config)
{
    services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    });
    services.AddApplication();
    services.AddPersistence(config["DbConnection"]);

    services.AddIdentity<User, IdentityRole<Guid>>(config =>
    {
        config.Password.RequireDigit = false;
        config.Password.RequireLowercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
        config.Password.RequiredLength = 4;
    })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    services.AddIdentityServer()
        .AddAspNetIdentity<User>()
        .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
        .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
        .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
        .AddInMemoryClients(IdentityConfiguration.Clients)
        .AddDeveloperSigningCredential();

    services.AddEndpointsApiExplorer();
    services.AddAuthentication("OAuth")
        .AddJwtBearer("OAuth", config =>
        {
            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);

            config.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Query.ContainsKey("access_token"))
                    {
                        context.Token = context.Request.Query["access_token"];
                    }

                    return Task.CompletedTask;
                }
            };

            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Constants.Issuer,
                ValidAudience = Constants.Audience,
                IssuerSigningKey = key
            };
        });

    services.AddSwaggerGen();
    services.AddControllers();

    services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
    services.AddApiVersioning();
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ATOH v1")));
    }

    app.UseCustomExceptionHandler();
    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
}