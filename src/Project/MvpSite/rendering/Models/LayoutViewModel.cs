using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Project.MvpSite.Models
{
    public class LayoutViewModel
    {
        [SitecoreRouteField]
        public TextField MenuTitle { get; set; }

        [SitecoreRouteField]
        public CheckboxField IncludeInMenu { get; set; }

        [SitecoreRouteField]
        public CheckboxField RequiresAuthentication { get; set; }

        [SitecoreRouteField]
        public TextField MetaKeywords { get; set; }

        [SitecoreRouteField]
        public TextField MetaDescription { get; set; }

        [SitecoreRouteField]
        public TextField OgTitle { get; set; }

        [SitecoreRouteField]
        public TextField OgDescription { get; set; }

        [SitecoreRouteField]
        public ImageField OgImage { get; set; }

        [SitecoreRouteField]
        public TextField OgType { get; set; }
    }
}
