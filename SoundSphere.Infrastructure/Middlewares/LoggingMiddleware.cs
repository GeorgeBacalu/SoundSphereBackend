using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace SoundSphere.Infrastructure.Middlewares
{
    public class LoggingMiddleware : IMiddleware
    {
        private Stopwatch stopwatch {  get; set; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine(context.Request.Path);
            stopwatch = Stopwatch.StartNew();
            await next(context);
            stopwatch.Stop();
            Console.WriteLine($"{context.Request.Path} - {stopwatch.ElapsedMilliseconds} ms\n");
        }
    }
}