using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mvp.Project.MvpSite.Middleware;
using Mvp.Project.MvpSite.Models;
using Okta.AspNetCore;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.LayoutService.Client.Exceptions;
using Sitecore.LayoutService.Client.Response.Model;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Project.MvpSite.Controllers
{
    public class DefaultController : Controller
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }

        // Inject Sitecore rendering middleware for this controller action
        // (enables model binding to Sitecore objects such as Route,
        // and causes requests to the Sitecore Layout Service for controller actions)
        [UseMvpSiteRendering]
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
                            _logger.LogError(error, error.Message);
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
            bool result = false;
            if (request.Response?.Content?.Sitecore?.Route?.Fields.TryGetValue("RequiresAuthentication", out IFieldReader requiresAuthFieldReader) ?? false)
            {
                result = requiresAuthFieldReader.Read<CheckboxField>().Value;
            }
            
            return result;
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
    }
}