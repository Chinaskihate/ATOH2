using System.Net;
using System.Text.Json;
using ATOH.Application.Common.Exceptions;

namespace ATOH.WebAPI.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (ex)
        {
            case ArgumentException argumentException:
                code = HttpStatusCode.BadRequest;
                break;
            case RevokedUserException revokedUserException:
                code = HttpStatusCode.Forbidden;
                break;
        }

        context.Response.ContentType = "Application/json";
        context.Response.StatusCode = (int)code;
        if (string.IsNullOrEmpty(result))
        {
            result = JsonSerializer.Serialize(new { error = ex.Message });
        }

        return context.Response.WriteAsync(result);
    }
}