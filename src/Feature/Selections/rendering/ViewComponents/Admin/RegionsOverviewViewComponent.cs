using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Admin;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Sitecore.AspNet.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Admin
{
    [ViewComponent(Name = ViewComponentName)]
    public class RegionsOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminRegionsOverview";

        public RegionsOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

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
                if (regionsResponse.StatusCode == HttpStatusCode.OK && regionsResponse.Result != null)
                {
                    model.List.AddRange(regionsResponse.Result);
                }
            }

            return View(model);
        }

        private void GenerateFakeDataForEdit(RegionsOverviewModel model)
        {
            Region region = new (1)
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
}
