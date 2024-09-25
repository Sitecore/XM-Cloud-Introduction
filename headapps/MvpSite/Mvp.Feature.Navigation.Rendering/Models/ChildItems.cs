using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp.Feature.Navigation.Models
{
    public class ChildItems<TFieldType>
    {
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? DisplayName { get; set; }

        [SitecoreComponentField]
        public TFieldType? Fields { get; set; }
    }
}
