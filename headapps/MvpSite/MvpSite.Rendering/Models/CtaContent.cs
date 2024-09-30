using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models;

public class CtaContent
{
    public TextField? CtaTitle { get; set; }

    public RichTextField? CtaText { get; set; }

    public HyperLinkField? CtaLink { get; set; }

    public ImageField? CtaImage { get; set; }
}