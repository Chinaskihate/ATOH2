using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ATOH.WebAPI.Options;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var apiVersion = description.ApiVersion.ToString();
            options.SwaggerDoc(description.GroupName,
                new OpenApiInfo()
                {
                    Version = apiVersion,
                    Title = $"ATOH Api {apiVersion}",
                    Description = "Test assignment",
                    TermsOfService = new Uri("https://github.com/Chinaskihate"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Telegram",
                        Url = new Uri("https://t.me/chinaskiHate")
                    }
                });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
                Name = "Authorization",
                Description = "Basic Authorization header using the Bearer scheme."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            options.CustomOperationIds(apiDecription =>
                apiDecription.TryGetMethodInfo(out MethodInfo methodInfo)
                    ? methodInfo.Name
                    : null);
        }
    }
}