using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Any;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Any;

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
            Response<User>? userResponse = null;
            if (model.IsEdit && ModelState.IsValid)
            {
                User updatedUser = new(Guid.Empty)
                {
                    Name = model.Name ?? string.Empty,
                    Email = model.Email ?? string.Empty,
                    Country = new Country(model.CountryId),

                    // Clear Twitter ImageType if selected, as Twitter API is no longer available
                    ImageType = SanitizeImageType(model.ImageType)
                };

                userResponse = await Client.UpdateCurrentUserAsync(updatedUser);
            }
            else if (!model.IsEdit)
            {
                userResponse = await Client.GetCurrentUserAsync();
            }

            if (userResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                User user = userResponse.Result;
                model.Name = user.Name;
                model.Email = user.Email;
                model.CountryId = user.Country?.Id ?? 0;

                // Clear Twitter ImageType if selected, as Twitter API is no longer available
                model.ImageType = SanitizeImageType(user.ImageType);
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

    /// Converts Twitter ImageType to Anonymous as Twitter API is no longer available
    private static ImageType SanitizeImageType(ImageType imageType)
    {
        return imageType == ImageType.Twitter ? ImageType.Anonymous : imageType;
    }

    private async Task LoadCountries(MyDataEditModel model)
    {
        Response<IList<Country>> countryResponse = await Client.GetCountriesAsync(1, short.MaxValue);
        if (countryResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Countries.AddRange(countryResponse.Result);
        }
    }

    private async Task LoadConsents(MyDataEditModel model)
    {
        Response<IList<Consent>> consentResponse = await Client.GetConsentsAsync();
        if (consentResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Consents.AddRange(consentResponse.Result);
        }
    }
}