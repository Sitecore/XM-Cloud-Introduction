using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Mvp.Feature.Navigation.Models
{
    public class ChildItems<FieldType>
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }

        [SitecoreComponentField]
        public FieldType Fields { get; set; }
    }
}
