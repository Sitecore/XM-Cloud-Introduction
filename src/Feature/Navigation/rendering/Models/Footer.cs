using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Navigation.Models
{
    public class Footer
    {
        [SitecoreComponentField(Name = "items")]
        public ChildItems<FooterFields>[] SocialLinks { get; set; }
    }
}

public class FooterFields
{
    [SitecoreComponentField]
    public HyperLinkField Link { get; set; }
    public TextField Icon { get; set; }
}