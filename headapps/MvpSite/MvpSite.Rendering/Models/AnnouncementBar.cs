using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models;

public class AnnouncementBar
{
    public RichTextField? AnnouncementText { get; set; }

    public HyperLinkField? AnnouncementLink { get; set; }
}