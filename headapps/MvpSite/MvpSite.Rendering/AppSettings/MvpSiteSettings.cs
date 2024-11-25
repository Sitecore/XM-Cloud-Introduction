namespace MvpSite.Rendering.AppSettings;

public class MvpSiteSettings
{
    public static readonly string Key = "Sitecore";

    public string? DefaultSiteName { get; set; }

    public string? NotFoundPage { get; set; }

    public string MvpProfilePageItemPath { get; set; } = "/Directory/Profile/*";

    public bool EnableEditingMode { get; set; }

    public string EditingSecret { get; set; } = string.Empty;

    public string? EdgeContextId { get; set; }

    public string? EditingPath { get; set; }

    public bool EnableLocalContainer { get; set; }

    public Uri? LocalContainerLayoutUri { get; set; }
}