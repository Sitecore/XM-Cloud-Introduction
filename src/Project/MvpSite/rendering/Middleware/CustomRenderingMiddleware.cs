using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Middleware;
using Sitecore.LayoutService.Client;
using Sitecore.LayoutService.Client.Exceptions;
using Sitecore.LayoutService.Client.Request;
using Sitecore.LayoutService.Client.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Middleware
{
    public class CustomRenderingEngineMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISitecoreLayoutRequestMapper _requestMapper;
        private readonly ISitecoreLayoutClient _layoutService;
        private readonly RenderingEngineOptions _options;
        private readonly IGraphQLClientFactory _graphQLClientFactory;
        private readonly IGraphQLRequestBuilder _graphQLRequestBuilder;
        private readonly IConfiguration _configuration;

        public CustomRenderingEngineMiddleware(RequestDelegate next, ISitecoreLayoutClient layoutService, ISitecoreLayoutRequestMapper requestMapper, IOptions<RenderingEngineOptions> options, IGraphQLClientFactory graphQLClientFactory, IGraphQLRequestBuilder graphQLRequestBuilder, IConfiguration configuration)
        {
            _next = next;
            _requestMapper = requestMapper;
            _options = options.Value;
            _graphQLClientFactory = graphQLClientFactory;
            _graphQLRequestBuilder = graphQLRequestBuilder;
            _layoutService = layoutService;
            _configuration= configuration;
        }              

        public async Task Invoke(HttpContext httpContext, IViewComponentHelper viewComponentHelper, IHtmlHelper htmlHelper)
        {
            SitecoreLayoutRequest sitecoreLayoutRequest = _requestMapper.Map(httpContext.Request);

            if (httpContext.GetSitecoreRenderingContext() == null)
            {
                SitecoreLayoutResponse response = await GetSitecoreLayoutResponse(httpContext).ConfigureAwait(continueOnCapturedContext: false);

                //Check not found page and set status code - start
                if (response.HasErrors)
                {
                    foreach (SitecoreLayoutServiceClientException error in response.Errors)
                    {
                        switch (error)
                        {
                            case ItemNotFoundSitecoreLayoutServiceClientException:
                                httpContext.Request.Path = _configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>().NotFoundPage;
                                sitecoreLayoutRequest = _requestMapper.Map(httpContext.Request);
                                response = await GetSitecoreLayoutResponse(httpContext).ConfigureAwait(continueOnCapturedContext: false);
                                if (response.Request.Path() == _configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>().NotFoundPage) { httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound; }
                                break;

                            default:
                                throw error;
                        }
                    }
                }
                //Check not found page and set status code - end

                SitecoreRenderingContext renderingContext = new()
                {
                    Response = response,
                    RenderingHelpers = new RenderingHelpers(viewComponentHelper, htmlHelper)
                };
                httpContext.SetSitecoreRenderingContext(renderingContext);
            }
            else
            {
                httpContext.GetSitecoreRenderingContext().RenderingHelpers = new RenderingHelpers(viewComponentHelper, htmlHelper);
            }

            foreach (Action<HttpContext> postRenderingAction in (IEnumerable<Action<HttpContext>>)_options.PostRenderingActions)
                postRenderingAction(httpContext);

            httpContext.Items.Add(nameof(RenderingEngineMiddleware), null);

            await _next(httpContext).ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task<SitecoreLayoutResponse> GetSitecoreLayoutResponse(HttpContext httpContext)
        {
            SitecoreLayoutRequest sitecoreLayoutRequest = _requestMapper.Map(httpContext.Request);
            return await _layoutService.Request(sitecoreLayoutRequest).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
