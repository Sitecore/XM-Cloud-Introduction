using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public abstract class BaseModel
{
    [SitecoreContextProperty]
    public bool IsEditing { get; set; }

    public List<string> ErrorMessages { get; set; } = [];
}