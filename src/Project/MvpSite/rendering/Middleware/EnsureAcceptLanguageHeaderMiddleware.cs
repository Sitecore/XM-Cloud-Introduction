using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Middleware
{
    public class EnsureAcceptLanguageHeaderMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly string _defaultLanguage = configuration.GetValue<string>("DefaultAcceptLanguageHeader");
        
        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Accept-Language"))
            {
                
                httpContext.Request.Headers.Append("Accept-Language", _defaultLanguage);
            }
            else
            {
                // NOTE [IVA] Hard fix for #555373 until V22 release
                httpContext.Request.Headers.AcceptLanguage = _defaultLanguage;
            }

            await next(httpContext);
        }
    }
}
