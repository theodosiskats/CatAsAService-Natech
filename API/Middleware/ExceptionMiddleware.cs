using System.Net;
using System.Text.Json;
using Persistence;

namespace API.Middleware;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env,
    DataContext context)
{
    private readonly DataContext _context = context;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            // Log the error
            logger.LogError(ex, ex.Message);

            // Check if the response has already started
            if (httpContext.Response.HasStarted)
            {
                logger.LogWarning("The response has already started, the exception middleware will not be executed");
                return;
            }

            // Set the response headers and status code
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Prepare the API response object
            var response = env.IsDevelopment()
                ? new ApiException(httpContext.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(httpContext.Response.StatusCode, "Internal Server Error", null);

            // Read request body for logging purposes
            if (httpContext.Request.Body.CanSeek) // Ensure the body can be read without causing issues
            {
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin); // Reset stream position
                using var reader = new StreamReader(httpContext.Request.Body);
                var requestBody = await reader.ReadToEndAsync();
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin); // Reset again for further use
            }

            // Log the event in the database
                
            // Serialize the response
            var json = JsonSerializer.Serialize(response);

            // Write the JSON response
            await httpContext.Response.WriteAsync(json);
        }
    }
}
