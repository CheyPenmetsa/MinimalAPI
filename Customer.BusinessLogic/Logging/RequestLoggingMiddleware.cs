using Microsoft.AspNetCore.Http;

namespace Customer.BusinessLogic.Logging
{
    public class RequestLoggingMiddleWare
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Request Logging : {context.Request.Method} {context.Request.Path}");

            await _next(context);
        }
    }
}
