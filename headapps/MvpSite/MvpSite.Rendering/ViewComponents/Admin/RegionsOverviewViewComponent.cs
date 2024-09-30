using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class RegionsOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminRegionsOverview";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        RegionsOverviewModel model = await ModelBinder.Bind<RegionsOverviewModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
        }
        else
        {
            Response<IList<Region>> regionsResponse = await Client.GetRegionsAsync(model.Page, model.PageSize);
            if (regionsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.List.AddRange(regionsResponse.Result);
            }
        }

        return View(model);
    }

    private static void GenerateFakeDataForEdit(RegionsOverviewModel model)
    {
        Region region = new(1)
        {
            Name = "Lorem"
        };
        region.Countries.Add(new Country(1) { Name = "ACountry" });
        model.List.Add(region);

        region = new Region(2)
        {
            Name = "Dolor"
        };
        region.Countries.Add(new Country(2) { Name = "BCountry" });
        region.Countries.Add(new Country(3) { Name = "CCountry" });
        model.List.Add(region);

        region = new Region(3)
        {
            Name = "Sid"
        };
        region.Countries.Add(new Country(4) { Name = "DCountry" });
        region.Countries.Add(new Country(5) { Name = "ECountry" });
        model.List.Add(region);
    }
}