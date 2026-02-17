using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class LicensesUploadViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminLicensesUpload";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        LicensesUploadModel model = await ModelBinder.Bind<LicensesUploadModel>(ViewContext);
        if (model.LicensesFile != null)
        {
            await using Stream stream = model.LicensesFile.OpenReadStream();
            await Client.UploadLicensesAsync(stream);
            model.Success = true;
        }

        return View(model);
    }
}