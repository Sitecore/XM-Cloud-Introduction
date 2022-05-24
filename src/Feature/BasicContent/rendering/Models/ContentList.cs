using Sitecore.AspNet.RenderingEngine.Binding;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.BasicContent.Models
{
    public class ContentList
    {
        [SitecoreComponentField(Name = "ContentListTitle")]
        public TextField Title { get; set; }

        [SitecoreComponentField(Name = "Selected Items")]
        public ContentListItem[] Items { get; set; }

        [SitecoreComponentProperty()] 
        public ItemLinkField ContentListType { get; set; }

        [SitecoreComponentField]
        public NumberField ContentListLimit { get; set; }
    }

    public class ContentListItem
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }

        [SitecoreComponentField]

        public ContentListItemFields Fields { get; set; }
    }

    public class ContentListItemFields
    {
        [SitecoreComponentField]
        public TextField ItemTitle { get; set; }

        [SitecoreComponentField]
        public TextField ItemSubtitle { get; set; }

        [SitecoreComponentField]
        public TextField ItemText { get; set; }

        [SitecoreComponentField]
        public HyperLinkField ItemLink { get; set; }
    }
}