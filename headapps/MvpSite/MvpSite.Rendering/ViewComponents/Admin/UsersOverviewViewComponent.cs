using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class UsersOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminUsersOverview";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        UsersOverviewModel model = await ModelBinder.Bind<UsersOverviewModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
            result = View(model);
        }
        else if (model is { RemoveUserId: not null, RemoveConfirmed: true })
        {
            // TODO [ILs] Implement remove user
            Response<bool> removeUserResponse = new() { Result = false }; ////await Client.RemoveUserAsync(model.RemoveUserId.Value);
            if (removeUserResponse.Result)
            {
                await LoadUsers(model);
                result = View(model);
            }
            else
            {
                model.ErrorMessages.Add(removeUserResponse.Message);
                result = View("~/Views/Shared/_Error.cshtml", model);
            }
        }
        else if (model is { RemoveUserId: not null, RemoveConfirmed: false })
        {
            Response<User> userResponse = await Client.GetUserAsync(model.RemoveUserId.Value);
            if (userResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.RemoveUser = userResponse.Result;
                result = View("Confirm", model);
            }
            else
            {
                model.ErrorMessages.Add(userResponse.Message);
                result = View("~/Views/Shared/_Error.cshtml", model);
            }
        }
        else
        {
            await LoadUsers(model);
            result = View(model);
        }

        return result;
    }

    private static void GenerateFakeDataForEdit(UsersOverviewModel model)
    {
        model.List.Add(new User(Guid.NewGuid())
        {
            Identifier = "LoremIpsum",
            Name = "Lorem Ipsum",
            Email = "lorem@ipsum.com",
            Country = new Country(1)
            {
                Name = "Country A"
            }
        });
        model.List.Add(new User(Guid.NewGuid())
        {
            Identifier = "DolorSid",
            Name = "Dolor Sid",
            Email = "dolor@sid.com",
            Country = new Country(1)
            {
                Name = "Country B"
            }
        });
        model.List.Add(new User(Guid.NewGuid())
        {
            Identifier = "AmetErgo",
            Name = "Amet Ergo",
            Email = "amet@ergo.com",
            Country = new Country(1)
            {
                Name = "Country A"
            }
        });
    }

    private async Task LoadUsers(UsersOverviewModel model)
    {
        Response<IList<User>> usersResponse = await Client.GetUsersAsync(page: model.Page, pageSize: model.PageSize);
        if (usersResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.List.AddRange(usersResponse.Result);
        }
    }
}