using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Navigation.Models
{
    public class MainNav 
    {
        [SitecoreComponentField]
        public MainNavItems[] Items { get; set; }
    }

    public class MainNavItems
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }

        [SitecoreComponentField]
        public MainNavItemFields Fields { get; set; }
    }

    public class MainNavItemFields
    {
        [SitecoreComponentField]
        public CheckboxField IncludeInMenu { get; set; }

        [SitecoreComponentField]
        public CheckboxField RequiresAuthentication { get; set; }

        public TextField MenuTitle { get; set; }
    }
}