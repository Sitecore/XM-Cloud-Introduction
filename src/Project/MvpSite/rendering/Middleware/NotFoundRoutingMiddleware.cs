using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.LayoutService.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Middleware;
using Sitecore.LayoutService.Client;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace Mvp.Project.MvpSite.Middleware
{
    public class NotFoundRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        
        private readonly ILogger<NotFoundRoutingMiddleware> _logger;

        private readonly MvpSiteSettings _settings;

        private readonly ISitecoreLayoutRequestMapper _requestMapper;
        
        private readonly ISitecoreLayoutClient _layoutService;
        
        private readonly IOptions<RenderingEngineOptions> _options;
        
        public NotFoundRoutingMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            ILogger<NotFoundRoutingMiddleware> logger,
            ISitecoreLayoutRequestMapper requestMapper,
            ISitecoreLayoutClient layoutService,
            IOptions<RenderingEngineOptions> options)
        {
            _next = next;
            _settings = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
            _logger = logger;
            _requestMapper = requestMapper;
            _layoutService = layoutService;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context, IViewComponentHelper viewComponentHelper, IHtmlHelper htmlHelper)
        {
            ISitecoreRenderingContext sitecoreContext = context.GetSitecoreRenderingContext();
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - False positive
            if (sitecoreContext != null && (sitecoreContext.Response?.HasErrors ?? false))
            {
                ItemNotFoundSitecoreLayoutServiceClientException notFound = sitecoreContext.Response.Errors
                    .OfType<ItemNotFoundSitecoreLayoutServiceClientException>().FirstOrDefault();
                if (notFound != null)
                {
                    // [IVA] Keep track of not found pages in info logs
                    _logger.LogInformation(notFound, notFound.Message);
                    
                    // [IVA] Change the request faking it towards the 404 page
                    context.Request.Path = _settings.NotFoundPage;

                    // [IVA] Force the response to use 404 status code
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    
                    // [IVA] Bust the RenderingEngine execution cache
                    context.Items.Remove(nameof(RenderingEngineMiddleware));
                    context.Features.Set<ISitecoreRenderingContext>(null);
                    
                    // [IVA] Finally we re-run the RenderingEngine
                    await new RenderingEngineMiddleware(_next, _requestMapper, _layoutService, _options).Invoke(context, viewComponentHelper, htmlHelper);
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
