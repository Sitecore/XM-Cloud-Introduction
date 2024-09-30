using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models;

public class HeroBase
{
    public TextField? HeroTitle { get; set; }

    public TextField? HeroSubtitle { get; set; }

    public TextField? HeroDescription { get; set; }

    public HyperLinkField? HeroLink { get; set; }

    public ImageField? HeroImage { get; set; }
}