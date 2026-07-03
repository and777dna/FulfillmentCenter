using FulfillmentCenter.Exceptions;

namespace FulfillmentCenter.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (OrderNotFoundException e)
        {
            var statusCode = httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(new { statusCode, e.Message });
            logger.LogWarning(e, e.Message);
        }
        catch (Exception e)
        {
            var statusCode = httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { statusCode, e.Message });
            logger.LogError(e, e.Message);
        }
    }
}
