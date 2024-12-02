using Microsoft.AspNetCore.Mvc;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Exceptions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Attributes;
using Sitecore.AspNetCore.SDK.RenderingEngine.Interfaces;

namespace Mvp2025.Site.Controllers;

public class DefaultController : Controller
{
    private readonly SitecoreSettings? _settings;
    private readonly ILogger<DefaultController> _logger;

    public DefaultController(ILogger<DefaultController> logger, IConfiguration configuration)
    {
        _settings = configuration.GetSection(SitecoreSettings.Key).Get<SitecoreSettings>();
        ArgumentNullException.ThrowIfNull(_settings);
        _logger = logger;
    }

    [UseSitecoreRendering]
    public IActionResult Index(Layout model)
    {
        IActionResult result = Empty;
        ISitecoreRenderingContext? request = HttpContext.GetSitecoreRenderingContext();
        if ((request?.Response?.HasErrors ?? false) && !IsPageEditingRequest(request))
        {
            foreach (SitecoreLayoutServiceClientException error in request.Response.Errors)
            {
                switch (error)
                {
                    case ItemNotFoundSitecoreLayoutServiceClientException:
                        result = View("NotFound");
                        break;
                    default:
                        _logger.LogError(error, "{Message}", error.Message);
                        throw error;
                }
            }
        }
        else
        {
            result = View(model);
        }
                
        return result;
    }

    public IActionResult Error()
    {
        return View();
    }

    private bool IsPageEditingRequest(ISitecoreRenderingContext request)
    {
        return request.Controller?.HttpContext.Request.Path == (_settings?.EditingPath ?? string.Empty);
    }
}