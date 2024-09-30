using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace MvpSite.Rendering.Models;

public class MainNavItemFields
{
    [SitecoreComponentField]
    public CheckboxField? IncludeInMenu { get; set; }

    [SitecoreComponentField]
    public CheckboxField? RequiresAuthentication { get; set; }

    public TextField? MenuTitle { get; set; }
}