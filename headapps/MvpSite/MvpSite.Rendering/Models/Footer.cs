using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models
{
    public class Footer
    {
        [SitecoreComponentField(Name = "Social Media Links")]
        public ChildItems<FooterFields>[]? SocialLinks { get; set; }

        [SitecoreComponentField(Name = "Content")]
        public RichTextField? CopyrightText { get; set; }
    }
}
