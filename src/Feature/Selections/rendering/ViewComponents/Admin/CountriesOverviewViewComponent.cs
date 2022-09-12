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
                Response<IList<Country>> regionsResponse = await Client.GetCountriesAsync(model.Page, model.PageSize);
                if (regionsResponse.StatusCode == HttpStatusCode.OK && regionsResponse.Result != null)
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
}
