using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class Rss
{
    [SitecoreComponentField]
    public TextField? Title { get; set; }

    [SitecoreComponentField]
    public TextField? Description { get; set; }

    [SitecoreComponentField]
    public TextField? RssUrl { get; set; }
}