using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookManagement.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var method = context.Request.Method;
            var path = context.Request.Path.ToString();

            Console.WriteLine($"[{time}] Method: {method} - Path: {path}");

            // Kiểm tra URL Detail
            if (path.StartsWith("/Book/Detail/"))
            {
                var idPart = path.Replace("/Book/Detail/", "");

                if (int.TryParse(idPart, out int id))
                {
                    if (id <= 0)
                    {
                        context.Response.StatusCode = 400;

                        await context.Response.WriteAsync("Book id khong hop le");

                        return;
                    }
                }
            }

            await _next(context);

            Console.WriteLine($"Status Code: {context.Response.StatusCode}");
        }
    }
}