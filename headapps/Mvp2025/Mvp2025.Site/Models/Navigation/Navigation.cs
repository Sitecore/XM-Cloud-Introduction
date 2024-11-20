using Sitecore.AspNetCore.SDK.LayoutService.Client.Serialization.Converter;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models.Navigation;

public class Navigation : BaseModel
{
    public Navigation() { }

    public Navigation(List<NavigationItem>? navigationItems, int? menuLevel)
    {
        NavigationItems = navigationItems;
        MenuLevel = menuLevel;
    }

    [SitecoreComponentField(Name = FieldParser.CustomContentFieldKey)]
    public List<NavigationItem>? NavigationItems { get; set; }

    public int? MenuLevel { get; set; }
}