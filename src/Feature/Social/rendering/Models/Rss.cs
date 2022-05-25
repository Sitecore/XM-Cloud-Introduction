using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;
using System.Collections.Generic;

namespace Mvp.Feature.Social.Models
{
    public class Rss
    {
        [SitecoreComponentField]
        public TextField Title { get; set; }

        [SitecoreComponentField]
        public TextField Description { get; set; }

        [SitecoreComponentField]
        public TextField RssUrl { get; set; }
    }
}
