using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.BasicContent.Models
{
    public class AnnouncementBar
    {
        public RichTextField? AnnouncementText { get; set; }
        public HyperLinkField? AnnouncementLink { get; set; }
    }
}
