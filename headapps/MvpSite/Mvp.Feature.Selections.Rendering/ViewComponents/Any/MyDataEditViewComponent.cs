using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Any;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Any
{
    [ViewComponent(Name = ViewComponentName)]
    public class MyDataEditViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
        : BaseViewComponent(modelBinder, client)
    {
        public const string ViewComponentName = "AnyMyDataEdit";

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            IViewComponentResult result;
            MyDataEditModel model = await ModelBinder.Bind<MyDataEditModel>(ViewContext);
            if (!model.IsEditing)
            {
                Response<User> userResponse = null;
                if (model.IsEdit && ModelState.IsValid)
                {
                    User updatedUser = new(Guid.Empty)
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Country = new Country(model.CountryId),
                        ImageType = model.ImageType
                    };

                    userResponse = await Client.UpdateCurrentUserAsync(updatedUser);
                }
                else if (!model.IsEdit)
                {
                    userResponse = await Client.GetCurrentUserAsync();
                }

                if (userResponse is { StatusCode: HttpStatusCode.OK, Result: { } })
                {
                    User user = userResponse.Result;
                    model.Name = user.Name;
                    model.Email = user.Email;
                    model.CountryId = user.Country?.Id ?? 0;
                    model.ImageType = user.ImageType;
                    model.ImageUri = user.ImageUri;
                    ModelState.Clear();
                }
                else if (userResponse != null && userResponse.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.TryAddModelError(string.Empty, userResponse.Message);
                }

                Task.WaitAll(LoadCountries(model), LoadConsents(model));
                if (model.IsEdit && ModelState.IsValid)
                {
                    result = View("Updated", model);
                }
                else
                {
                    result = View(model);
                }
            }
            else
            {
                result = View("ExperienceEditor", model);
            }

            return result;
        }

        private async Task LoadCountries(MyDataEditModel model)
        {
            Response<IList<Country>> countryResponse = await Client.GetCountriesAsync(1, short.MaxValue);
            if (countryResponse.StatusCode == HttpStatusCode.OK && countryResponse.Result != null)
            {
                model.Countries.AddRange(countryResponse.Result);
            }
        }

        private async Task LoadConsents(MyDataEditModel model)
        {
            Response<IList<Consent>> consentResponse = await Client.GetConsentsAsync();
            if (consentResponse.StatusCode == HttpStatusCode.OK && consentResponse.Result != null)
            {
                model.Consents.AddRange(consentResponse.Result);
            }
        }
    }
}
