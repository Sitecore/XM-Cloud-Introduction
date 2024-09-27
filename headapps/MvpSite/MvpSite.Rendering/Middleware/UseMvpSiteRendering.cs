using Microsoft.AspNetCore.Mvc;

namespace MvpSite.Rendering.Middleware;

public class UseMvpSiteRendering(Type configurationType)
    : MiddlewareFilterAttribute(configurationType)
{
    public UseMvpSiteRendering()
        : this(typeof(MvpSiteRenderingEnginePipeline))
    {
    }
}