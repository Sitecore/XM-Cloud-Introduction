using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace Mvp2025.Site.Models.Navigation
{
    public class NavigationItem
    {
        public string? Id { get; set; }

        public List<string>? Styles { get; set; } = [];

        public List<NavigationItem>? Children { get; set; } = [];

        public string? Href { get; set; }

        public string? QueryString { get; set; }

        public TextField? NavigationTitle { get; set; }
    }
}