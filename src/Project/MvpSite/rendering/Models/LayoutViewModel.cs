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
    }
}
