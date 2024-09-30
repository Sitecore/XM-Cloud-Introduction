using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MvpSite.Rendering.Middleware;
using MvpSite.Rendering.Models;
using Okta.AspNetCore;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Exceptions;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Interfaces;

namespace MvpSite.Rendering.Controllers;

public class DefaultController(ILogger<DefaultController> logger)
    : Controller
{
    // Inject Sitecore rendering middleware for this controller action
    // (enables model binding to Sitecore objects such as Route,
    // and causes requests to the Sitecore Layout Service for controller actions)
    [UseMvpSiteRendering]
    public IActionResult? Index(LayoutViewModel model)
    {
        IActionResult? result = null;
        ISitecoreRenderingContext? request = HttpContext.GetSitecoreRenderingContext();
        if (request?.Response?.HasErrors ?? false)
        {
            foreach (SitecoreLayoutServiceClientException error in request.Response.Errors)
            {
                switch (error)
                {
                    default:
                        logger.LogError(error, "{Message}", error.Message);
                        throw error;
                }
            }
        }
        else if (!(HttpContext.User.Identity?.IsAuthenticated ?? false) && IsSecurePage(request) && !(request?.Response?.Content?.Sitecore?.Context?.IsEditing ?? false))
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new LayoutViewModel
        {
            MenuTitle = new TextField("Error")
        });
    }

    // ReSharper disable once IdentifierTypo - Common endpoint name
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Healthz()
    {
        // TODO: Do we want to add logic here to confirm connectivity with SC?
        return Ok("Healthy");
    }

    private static bool IsSecurePage(ISitecoreRenderingContext? request)
    {
        bool result = false;
        if (request?.Response?.Content?.Sitecore?.Route?.Fields.TryGetValue("RequiresAuthentication", out IFieldReader? requiresAuthFieldReader) ?? false)
        {
            result = requiresAuthFieldReader.Read<CheckboxField>()?.Value ?? false;
        }

        return result;
    }
}