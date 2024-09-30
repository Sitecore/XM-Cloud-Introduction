using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class ContentListItem
{
    public string? Url { get; set; }

    public string? Name { get; set; }

    public string? Id { get; set; }

    public string? DisplayName { get; set; }

    [SitecoreComponentField]
    public ContentListItemFields? Fields { get; set; }
}
