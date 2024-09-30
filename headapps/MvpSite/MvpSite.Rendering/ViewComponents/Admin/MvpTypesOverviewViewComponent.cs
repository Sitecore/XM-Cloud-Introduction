using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class MvpTypesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminMvpTypesOverview";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        MvpTypesOverviewModel model = await ModelBinder.Bind<MvpTypesOverviewModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
        }
        else
        {
            Response<IList<MvpType>> mvpTypesResponse = await Client.GetMvpTypesAsync(model.Page, model.PageSize);
            if (mvpTypesResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.List.AddRange(mvpTypesResponse.Result);
            }
        }

        return View(model);
    }

    private static void GenerateFakeDataForEdit(MvpTypesOverviewModel model)
    {
        model.List.Add(new MvpType(1)
        {
            Name = "Lorem"
        });
        model.List.Add(new MvpType(2)
        {
            Name = "Ipsum"
        });
        model.List.Add(new MvpType(3)
        {
            Name = "Dolor"
        });
        model.List.Add(new MvpType(4)
        {
            Name = "Sid"
        });
        model.List.Add(new MvpType(5)
        {
            Name = "Amet"
        });
    }
}