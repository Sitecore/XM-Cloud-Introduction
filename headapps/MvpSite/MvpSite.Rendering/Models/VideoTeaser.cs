using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models;

public class VideoTeaser
{
    public TextField? TeaserTitle { get; set; }

    public RichTextField? TeaserText { get; set; }

    public HyperLinkField? TeaserLink { get; set; }

    public TextField? TeaserEmbed { get; set; }
}