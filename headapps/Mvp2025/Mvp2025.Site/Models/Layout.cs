using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models;

public class Layout
{
    [SitecoreRouteField]
    public string? Name { get; set; }

    [SitecoreRouteField]
    public string? DisplayName { get; set; }

    [SitecoreRouteField]
    public string? ItemId { get; set; }

    [SitecoreRouteField]
    public string? ItemLanguage { get; set; }

    [SitecoreRouteField]
    public string? TemplateId { get; set; }

    [SitecoreRouteField]
    public string? TemplateName { get; set; }
}
