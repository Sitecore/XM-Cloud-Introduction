using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class MainNav
{
    [SitecoreComponentField]
    public MainNavItems[]? Items { get; set; }
}