using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp.Feature.Navigation.Models
{
    public class Footer
    {
        [SitecoreComponentField(Name = "Social Media Links")]
        public ChildItems<FooterFields>[]? SocialLinks { get; set; }

        [SitecoreComponentField(Name = "Content")]
        public RichTextField? CopyrightText { get; set; }
    }
}

public class FooterFields
{
    [SitecoreComponentField]
    public HyperLinkField? Link { get; set; }
    public TextField? Icon { get; set; }
}