using Microsoft.AspNetCore.Authentication;
using Mvp.Selections.Client.Interfaces;

namespace MvpSite.Rendering.Providers;

public class HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    public async Task<string> GetTokenAsync()
    {
        string result = string.Empty;
        HttpContext? context = httpContextAccessor.HttpContext;
        if (context != null)
        {
            result = await context.GetTokenAsync("id_token") ?? string.Empty;
        }

        return result;
    }
}