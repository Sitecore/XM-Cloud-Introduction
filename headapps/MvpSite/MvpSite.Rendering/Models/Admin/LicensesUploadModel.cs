using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin;

public class LicensesUploadModel : BaseModel
{
    public TextField? TitleLabel { get; set; }

    public TextField? UploadLabel { get; set; }

    public RichTextField? SuccessText { get; set; }

    public IFormFile? LicensesFile { get; set; }

    public bool Success { get; set; }
}