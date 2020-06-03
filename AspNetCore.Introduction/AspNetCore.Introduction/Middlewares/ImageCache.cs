using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Introduction.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ImageCache
    {
        private readonly RequestDelegate _next;

        public ImageCache(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var currentContext = httpContext;
            await _next(httpContext);
            var currentContext5 = httpContext;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ImageCacheExtensions
    {
        public static IApplicationBuilder UseImageCache(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageCache>();
        }
    }
}
