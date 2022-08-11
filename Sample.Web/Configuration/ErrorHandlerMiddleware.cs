using Sample.Application;
using System.Net;

namespace Sample.Web.Configuration
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                switch (error.InnerException)
                {
                    case NoneAppSessionException:
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsJsonAsync(error.Message);
                        break;
                    default:
                        throw;
                }
            }
        }
    }
}