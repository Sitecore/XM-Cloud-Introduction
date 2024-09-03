using System.Collections.Generic;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp.Feature.People.Models
{
    public abstract class BaseViewModel
    {
        [SitecoreContextProperty]
        public bool IsEditing { get; set; }

        public List<string> ErrorMessages { get; set; } = [];
    }
}
