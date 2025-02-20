using Microsoft.AspNetCore.Builder;

namespace Core.Packages.Persistence.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static void UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
