namespace FulfillmentCenter.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception e)
        {
            var statusCode = httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsJsonAsync(new {statusCode, e.Message});
        }
    }
}