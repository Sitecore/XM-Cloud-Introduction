using System;
using Microsoft.AspNetCore.Mvc;

namespace Mvp.Project.MvpSite.Middleware
{
    public class UseMvpSiteRendering(Type configurationType)
        : MiddlewareFilterAttribute(configurationType)
    {
        public UseMvpSiteRendering()
        : this(typeof(MvpSiteRenderingEnginePipeline))
        {
        }
    }
}
