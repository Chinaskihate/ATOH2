using System.Reflection;
using System.Text;
using ATOH.WebAPI;
using ATOH.WebAPI.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services, builder.Configuration);
var app = builder.Build();
Configure(app, app.Environment);

app.Run();

void RegisterServices(IServiceCollection services, IConfiguration config)
{
    services.AddEndpointsApiExplorer();
    services.AddAuthentication("OAuth")
        .AddJwtBearer("OAuth", config =>
        {
            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);

            config.Events = new JwtBearerEvents()
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

            config.TokenValidationParameters = new TokenValidationParameters()
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