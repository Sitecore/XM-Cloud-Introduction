using Microsoft.AspNetCore.Mvc;
using MvpSite.Rendering.Middleware;
using MvpSite.Rendering.Models;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Exceptions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Interfaces;

namespace MvpSite.Rendering.Controllers;

public class MvpProfileController(ILogger<MvpProfileController> logger)
    : Controller
{
    [UseMvpProfileRendering]
    public IActionResult? Index(string name, LayoutViewModel model)
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
        else
        {
            result = View("~/Views/Default/Index.cshtml", model);
        }

        return result;
    }
}