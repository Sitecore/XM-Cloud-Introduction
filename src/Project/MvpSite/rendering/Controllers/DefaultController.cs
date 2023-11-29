using Microsoft.AspNetCore.Mvc;
using Mvp.Project.MvpSite.Models;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.AspNet.RenderingEngine.Filters;
using Sitecore.LayoutService.Client.Exceptions;
using Sitecore.LayoutService.Client.Response.Model.Fields;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Okta.AspNetCore;

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
        [UseSitecoreRendering]
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
                        case ItemNotFoundSitecoreLayoutServiceClientException notFound:
                            _logger.LogInformation(notFound, notFound.Message);
                            result = Render404();
                            break;
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

        private IActionResult Render404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFound", new LayoutViewModel { MenuTitle = new TextField("Not Found") });
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
    }
}