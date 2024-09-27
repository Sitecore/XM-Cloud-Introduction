using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class CountriesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminCountriesOverview";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        CountriesOverviewModel model = await ModelBinder.Bind<CountriesOverviewModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
        }
        else
        {
            Response<IList<Country>> regionsResponse = await Client.GetCountriesAsync(model.Page, model.PageSize);
            if (regionsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.List.AddRange(regionsResponse.Result);
            }
        }

        return View(model);
    }

    private static void GenerateFakeDataForEdit(CountriesOverviewModel model)
    {
        model.List.Add(new Country(1) { Name = "Lorem" });
        model.List.Add(new Country(2) { Name = "Ipsum" });
        model.List.Add(new Country(3) { Name = "Dolor" });
        model.List.Add(new Country(4) { Name = "Sid" });
        model.List.Add(new Country(5) { Name = "Amet" });
    }
}