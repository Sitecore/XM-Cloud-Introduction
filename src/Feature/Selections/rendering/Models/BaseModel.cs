using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Mvp.Feature.Selections.Models
{
    public abstract class BaseModel
    {
        [SitecoreContextProperty]
        public bool IsEditing { get; set; }
    }
}
