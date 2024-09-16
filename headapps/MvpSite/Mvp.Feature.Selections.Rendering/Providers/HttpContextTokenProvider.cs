using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Mvp.Selections.Client.Interfaces;

namespace Mvp.Feature.Selections.Providers
{
    public class HttpContextTokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTokenAsync()
        {
            string result = string.Empty;
            HttpContext context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                result = await context.GetTokenAsync("id_token");
            }

            return result;
        }
    }
}
