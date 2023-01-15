using System.Collections.Generic;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Mvp.Feature.Selections.Models
{
    public abstract class BaseModel
    {
        [SitecoreContextProperty]
        public bool IsEditing { get; set; }

        public List<string> ErrorMessages { get; set; } = new ();
    }
}
