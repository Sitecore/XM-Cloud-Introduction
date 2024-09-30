using Microsoft.AspNetCore.Mvc;

namespace MvpSite.Rendering.Middleware;

public class UseMvpProfileRendering(Type configurationType)
    : MiddlewareFilterAttribute(configurationType)
{
    public UseMvpProfileRendering()
        : this(typeof(MvpProfileRenderingEnginePipeline))
    {
    }
}