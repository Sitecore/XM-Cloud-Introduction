using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class ContentList
{
    [SitecoreComponentField(Name = "ContentListTitle")]
    public TextField? Title { get; set; }

    [SitecoreComponentField(Name = "Selected Items")]
    public ContentListItem[]? Items { get; set; }

    [SitecoreComponentProperty]
    public ItemLinkField? ContentListType { get; set; }

    [SitecoreComponentField]
    public NumberField? ContentListLimit { get; set; }
}