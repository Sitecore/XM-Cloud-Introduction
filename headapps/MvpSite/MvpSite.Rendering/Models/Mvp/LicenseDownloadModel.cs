using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Mvp;

public class LicenseDownloadModel : BaseModel
{
    public bool IsCurrentMvp { get; set; }

    public TextField? TitleLabel { get; set; }

    public HyperLinkField? DownloadMyLicenseLink { get; set; }
}