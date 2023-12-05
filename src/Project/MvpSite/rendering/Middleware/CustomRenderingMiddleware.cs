using Microsoft.AspNetCore.Http;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.LayoutService.Client;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Sitecore.LayoutService.Client.Response;
using System;
using Sitecore.LayoutService.Client.Request;
using System.Collections.Generic;
using Sitecore.AspNet.RenderingEngine.Middleware;
using Mvp.Foundation.DataFetching.GraphQL;
using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using System.Net;

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
                if (response.Request.Path() == _configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>().NotFoundPage) { httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound; }
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

            httpContext.Items.Add((object)nameof(RenderingEngineMiddleware), (object)null);

            await _next(httpContext).ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task<SitecoreLayoutResponse> GetSitecoreLayoutResponse(HttpContext httpContext)
        {
            //intercept for not found logic - start
            CustomGraphQlLayoutServiceHandler customGraphQlLayoutServiceHandler = new(_configuration,_graphQLRequestBuilder, _graphQLClientFactory);
            SitecoreLayoutRequest sitecoreLayoutRequest = _requestMapper.Map(httpContext.Request);
            bool retVal = await customGraphQlLayoutServiceHandler.CheckLayoutExists(sitecoreLayoutRequest);
            if (!retVal)  httpContext.Request.Path = _configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>().NotFoundPage;
            //intercept for not found logic - end

            sitecoreLayoutRequest = _requestMapper.Map(httpContext.Request);
            return await _layoutService.Request(sitecoreLayoutRequest).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
