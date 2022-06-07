using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Navigation.Models
{
    public class TopLinks
    {
        [SitecoreComponentField]
        public TopLink[] Items { get; set; }
    }

    public class TopLink
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public TopLinkFields Fields { get; set; }
    }
    public class TopLinkFields
    {
        public HyperLinkField Link { get; set; }
    }
}