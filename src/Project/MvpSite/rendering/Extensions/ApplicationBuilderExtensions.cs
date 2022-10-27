using Microsoft.AspNetCore.Builder;
using Mvp.Project.MvpSite.Middleware;

namespace Mvp.Project.MvpSite.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureAcceptLanguageHeader(this IApplicationBuilder app)
        {
            app.UseMiddleware<EnsureAcceptLanguageHeaderMiddleware>();
            return app;
        }
    }
}
