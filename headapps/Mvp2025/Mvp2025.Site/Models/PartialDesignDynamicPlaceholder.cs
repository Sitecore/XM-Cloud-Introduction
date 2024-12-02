using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models;

public class PartialDesignDynamicPlaceholder : BaseModel
{
    [SitecoreComponentParameter(Name ="sig")]
    public string? Sig { get; set; }
}
