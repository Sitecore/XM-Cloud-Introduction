using Microsoft.AspNetCore.Mvc;
using System;

namespace Mvp.Feature.People.Middleware
{
    public class UseMvpProfileRendering(Type configurationType)
        : MiddlewareFilterAttribute(configurationType)
    {
        public UseMvpProfileRendering()
            : this(typeof(MvpProfileRenderingEnginePipeline))
        {
        }
    }
}
