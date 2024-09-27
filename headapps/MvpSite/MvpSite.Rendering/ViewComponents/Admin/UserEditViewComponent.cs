using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class UserEditViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminUserEdit";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        UserEditModel model = await ModelBinder.Bind<UserEditModel>(ViewContext);
        if (!model.IsEditing)
        {
            Response<User>? userResponse = null;
            if (model.IsEdit && ModelState.IsValid)
            {
                User updatedUser = new(model.Id)
                {
                    Identifier = model.Identifier!,
                    Name = model.Name,
                    Email = model.Email,
                    Country = new Country(model.CountryId)
                };

                userResponse = await Client.UpdateUserAsync(updatedUser);
            }
            else if (!model.IsEdit)
            {
                userResponse = await Client.GetUserAsync(model.Id);
            }

            if (userResponse is { StatusCode: HttpStatusCode.OK, Result: { } })
            {
                User user = userResponse.Result;
                model.Identifier = user.Identifier;
                model.Name = user.Name;
                model.Email = user.Email;
                model.CountryId = user.Country?.Id ?? 0;
                ModelState.Clear();
            }
            else if (userResponse != null && userResponse.StatusCode != HttpStatusCode.OK)
            {
                ModelState.TryAddModelError(string.Empty, userResponse.Message);
            }

            Response<IList<Country>> countryResponse = await Client.GetCountriesAsync(1, short.MaxValue);
            if (countryResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.Countries.AddRange(countryResponse.Result);
            }

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
            result = View(model);
        }

        return result;
    }
}