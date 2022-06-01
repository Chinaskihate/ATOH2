namespace ATOH.WebAPI.Middleware;

/// <summary>
/// Extension class for custom middleware for exception handling.
/// </summary>
public static class CustomExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    /// Use custom exception handler.
    /// </summary>
    /// <param name="builder"> Application builder. </param>
    /// <returns> Application builder. </returns>
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}