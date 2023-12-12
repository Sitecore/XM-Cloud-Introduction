using Microsoft.AspNetCore.Mvc;
using Mvp.Project.MvpSite.Models;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.AspNet.RenderingEngine.Filters;
using Sitecore.LayoutService.Client.Exceptions;
using Sitecore.LayoutService.Client.Response.Model.Fields;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Okta.AspNetCore;
using Mvp.Project.MvpSite.Middleware;
using Microsoft.AspNetCore.Http;
using Mvp.Project.MvpSite.Rendering;

namespace Mvp.Project.MvpSite.Controllers
{
    public class DefaultController : Controller
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly SitemapBuilder _sitemap;

        public DefaultController(ILogger<DefaultController> logger, SitemapBuilder sitemap)
        {
            _logger = logger;
            _sitemap = sitemap;
        }

        // Inject Sitecore rendering middleware for this controller action
        // (enables model binding to Sitecore objects such as Route,
        // and causes requests to the Sitecore Layout Service for controller actions)
        [UseSitecoreRendering(typeof(CustomRenderingEnginePipeline))]
        public IActionResult Index(LayoutViewModel model)
        {
            IActionResult result = null;
            ISitecoreRenderingContext request = HttpContext.GetSitecoreRenderingContext();
            if (request.Response?.HasErrors ?? false)
            {
                foreach (SitecoreLayoutServiceClientException error in request.Response.Errors)
                {
                    switch (error)
                    {
                        default:
                            throw error;
                    }
                }
            }
            else if (!(HttpContext.User.Identity?.IsAuthenticated ?? false) && IsSecurePage(request) && !(request.Response?.Content?.Sitecore?.Context?.IsEditing ?? false))
            {
                AuthenticationProperties properties = new()
                {
                    RedirectUri = HttpContext.Request.GetEncodedUrl()
                };

                result = Challenge(properties, OktaDefaults.MvcAuthenticationScheme);
            }
            else
            {
                result = View(model);
            }

            return result;
        }

        private static bool IsSecurePage(ISitecoreRenderingContext request)
        {
            return request.Response?.Content?.Sitecore?.Route?.Fields["RequiresAuthentication"].Read<CheckboxField>().Value ?? false;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new LayoutViewModel
            {
                MenuTitle = new TextField("Error")
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Healthz()
        {
            // TODO: Do we want to add logic here to confirm connectivity with SC etc?
            return Ok("Healthy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IResult SiteMap()
        {
            var xml =  _sitemap.GenerateAsync();
            return Results.Stream(xml.Result, "text/xml");
        }
    }
}