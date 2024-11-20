using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models;

public class PageContent : BaseModel
{
    [SitecoreRouteField(Name = "Content")]
    public RichTextField? RouteContent { get; set; }

    [SitecoreComponentField(Name = "Content")]
    public RichTextField? ComponentContent { get; set; }

    public RichTextField? Content
    {
        get
        {
            return ComponentContent ?? RouteContent;
        }
    }
}
