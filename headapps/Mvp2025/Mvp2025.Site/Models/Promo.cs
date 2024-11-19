using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models;

public class Promo : BaseModel
{
    public const string VARIANT_WITH_TEXT = "WithText";

    [SitecoreComponentField]
    public ImageField? PromoIcon { get; set; }

    [SitecoreComponentField]
    public TextField? PromoText { get; set; }

    [SitecoreComponentField]
    public HyperLinkField? PromoLink { get; set; }
}
