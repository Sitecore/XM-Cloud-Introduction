using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class TopLinks
{
    [SitecoreComponentField]
    public TopLink[]? Items { get; set; }
}