using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using MvpSite.Rendering.Middleware;
using MvpSite.Rendering.Models;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Exceptions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Interfaces;
using File = Mvp.Selections.Client.Models.File;

namespace MvpSite.Rendering.Controllers;

public class MvpProfileController(ILogger<MvpProfileController> logger, MvpSelectionsApiClient client)
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

    public async Task<IActionResult?> DownloadLicense()
    {
        IActionResult result;
        Response<File> licenseResponse = await client.GetValidLicenseForCurrentUserAsync();
        if (licenseResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            result = new FileStreamResult(licenseResponse.Result.Content, licenseResponse.Result.ContentType)
            {
                FileDownloadName = licenseResponse.Result.FileName
            };
        }
        else
        {
            result = new NotFoundResult();
        }

        return result;
    }
}