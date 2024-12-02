using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models;

public class ColumnSplitter : RowSplitter
{
    [SitecoreComponentParameter]
    public string? ColumnWidth1 { get; set; }
    [SitecoreComponentParameter]
    public string? ColumnWidth2 { get; set; }
    [SitecoreComponentParameter]
    public string? ColumnWidth3 { get; set; }
    [SitecoreComponentParameter]
    public string? ColumnWidth4 { get; set; }
    [SitecoreComponentParameter]
    public string? ColumnWidth5 { get; set; }
    [SitecoreComponentParameter]
    public string? ColumnWidth6 { get; set; }
    [SitecoreComponentParameter]
    public string? ColumnWidth7 { get; set; }
    [SitecoreComponentParameter]
    public string? ColumnWidth8 { get; set; }

    public string[] ColumnWidths
    {
        get
        {
            return
            [
                ColumnWidth1 ?? string.Empty,
                ColumnWidth2 ?? string.Empty,
                ColumnWidth3 ?? string.Empty,
                ColumnWidth4 ?? string.Empty,
                ColumnWidth5 ?? string.Empty,
                ColumnWidth6 ?? string.Empty,
                ColumnWidth7 ?? string.Empty,
                ColumnWidth8 ?? string.Empty,
            ];
        }
    }
}
