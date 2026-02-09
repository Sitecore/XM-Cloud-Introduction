using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Mvp;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Mvp;

[ViewComponent(Name = ViewComponentName)]
public class MvpLicenseDownloadViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "MvpLicenseDownload";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        LicenseDownloadModel model = await ModelBinder.Bind<LicenseDownloadModel>(ViewContext);
        if (!model.IsEditing)
        {
            model.IsCurrentMvp = await IsCurrentUserAnMvp();
            result = View(model);
        }
        else
        {
            result = View("ExperienceEditor", model);
        }

        return result;
    }

    private async Task<bool> IsCurrentUserAnMvp()
    {
        bool result = false;
        Response<User> currentUserResponse = await client.GetCurrentUserAsync();
        if (currentUserResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            Response<MvpProfile> mvpProfileResponse = await client.GetMvpProfileAsync(currentUserResponse.Result.Id);
            if (mvpProfileResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                if (mvpProfileResponse.Result.Titles.Any(t => t.Application.Selection.Year == DateTime.UtcNow.Year))
                {
                    result = true;
                }
            }
        }

        return result;
    }
}