using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class ContentListItemFields
{
    [SitecoreComponentField]
    public TextField? ItemTitle { get; set; }

    [SitecoreComponentField]
    public TextField? ItemSubtitle { get; set; }

    [SitecoreComponentField]
    public TextField? ItemText { get; set; }

    [SitecoreComponentField]
    public HyperLinkField? ItemLink { get; set; }
}