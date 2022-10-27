using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Middleware
{
    public class EnsureAcceptLanguageHeaderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        public EnsureAcceptLanguageHeaderMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Accept-Language"))
            {
                var defaultLang = configuration.GetValue<string>("DefaultAcceptLanguageHeader");
                httpContext.Request.Headers.Add("Accept-Language", defaultLang);
            }

            await next(httpContext).ConfigureAwait(false);
        }
    }
}
