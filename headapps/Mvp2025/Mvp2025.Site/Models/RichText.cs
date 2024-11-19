using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models;

public class RichText : BaseModel
{
    [SitecoreComponentField]
    public RichTextField? Text { get; set; }
}
