using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class FooterFields
{
    [SitecoreComponentField]
    public HyperLinkField? Link { get; set; }

    public TextField? Icon { get; set; }
}