using Microsoft.AspNetCore.Builder;
using Mvp.Feature.People.Middleware;

namespace Mvp.Feature.People.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMvpProfileRouting(this IApplicationBuilder app)
        {
            app.UseMiddleware<MvpProfileRoutingMiddleware>();
            return app;
        }
    }
}
