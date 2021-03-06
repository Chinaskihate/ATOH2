using System.Reflection;
using System.Text;
using ATOH.Persistence;
using ATOH.WebAPI;
using ATOH.WebAPI.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    services.AddPersistence(config["DbConnection"]);

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

    //services.AddAuthentication("BasicAuthentication")
    //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ATOH v1")));
    }

    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
}