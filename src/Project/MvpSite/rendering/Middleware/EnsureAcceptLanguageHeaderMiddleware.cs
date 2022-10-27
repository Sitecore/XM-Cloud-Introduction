using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Middleware
{
    public class EnsureAcceptLanguageHeaderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingEngineMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware to call.</param>
        /// <param name="requestMapper">The <see cref="ISitecoreLayoutRequestMapper"/> to map the HttpRequest to a Layout Service request.</param>
        /// <param name="layoutService">The layout service client.</param>
        /// <param name="options">Rendering Engine options.</param>
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
