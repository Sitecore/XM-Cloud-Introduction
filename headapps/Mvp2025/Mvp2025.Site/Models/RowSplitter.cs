using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models;

public class RowSplitter : BaseModel
{
    [SitecoreComponentParameter]
    public string? EnabledPlaceholders { get; set; }
    [SitecoreComponentParameter]
    public string? Styles1 { get; set; }
    [SitecoreComponentParameter]
    public string? Styles2 { get; set; }
    [SitecoreComponentParameter]
    public string? Styles3 { get; set; }
    [SitecoreComponentParameter]
    public string? Styles4 { get; set; }
    [SitecoreComponentParameter]
    public string? Styles5 { get; set; }
    [SitecoreComponentParameter]
    public string? Styles6 { get; set; }
    [SitecoreComponentParameter]
    public string? Styles7 { get; set; }
    [SitecoreComponentParameter]
    public string? Styles8 { get; set; }

    public string[] ColumnStyles
    {
        get
        {
            return
            [
                Styles1 ?? string.Empty,
                Styles2 ?? string.Empty,
                Styles3 ?? string.Empty,
                Styles4 ?? string.Empty,
                Styles5 ?? string.Empty,
                Styles6 ?? string.Empty,
                Styles7 ?? string.Empty,
                Styles8 ?? string.Empty,
            ];
        }
    }

    public int[] EnabledPlaceholderIds
{
        get
        {
            return EnabledPlaceholders?.Split(',').Select(int.Parse).ToArray() ?? [];
        }
    }
}
