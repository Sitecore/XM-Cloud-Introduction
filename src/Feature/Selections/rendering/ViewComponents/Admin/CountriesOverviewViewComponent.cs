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
    public class CountriesOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminCountriesOverview";

        public CountriesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            CountriesOverviewModel model = await ModelBinder.Bind<CountriesOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                Response<IList<Country>> regionsResponse = await Client.GetCountriesAsync(await GetCurrentTokenAsync(), model.Page, model.PageSize);
                if (regionsResponse.StatusCode == HttpStatusCode.OK && regionsResponse.Result != null)
                {
                    model.Countries.AddRange(regionsResponse.Result);
                }
            }

            return View(model);
        }

        private void GenerateFakeDataForEdit(CountriesOverviewModel model)
        {
            model.Countries.Add(new Country(1) { Name = "Lorem" });
            model.Countries.Add(new Country(2) { Name = "Ipsum" });
            model.Countries.Add(new Country(3) { Name = "Dolor" });
            model.Countries.Add(new Country(4) { Name = "Sid" });
            model.Countries.Add(new Country(5) { Name = "Amet" });
        }
    }
}
