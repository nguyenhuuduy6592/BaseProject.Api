using Microsoft.AspNetCore.Builder;
using BaseProject.Api.Infrastructure.Middlewares;

namespace BaseProject.Api.Infrastructure.ConfigRegister
{
    public static class CutomMiddlewareConfig
    {
        public static void UseCutomMiddlewareConfig(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
