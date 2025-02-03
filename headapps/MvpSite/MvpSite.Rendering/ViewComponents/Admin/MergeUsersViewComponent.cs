using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Extensions;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class MergeUsersViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminMergeUsers";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        MergeUsersModel model = await ModelBinder.Bind<MergeUsersModel>(ViewContext);
        Response<User> userResponse = await Client.GetCurrentUserAsync();
        if (userResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            User currentUser = userResponse.Result;
            if (currentUser.HasRight(Right.Admin))
            {
                if (!model.IsMerging && (!string.IsNullOrWhiteSpace(model.OldUserNameSearch) || !string.IsNullOrWhiteSpace(model.OldUserEmailSearch)))
                {
                    model.OldUserOptions.AddRange(
                        await SearchUsersAsync(model, model.OldUserNameSearch, model.OldUserEmailSearch));
                }

                if (!model.IsMerging && (!string.IsNullOrWhiteSpace(model.TargetUserNameSearch) || !string.IsNullOrWhiteSpace(model.TargetUserEmailSearch)))
                {
                    model.TargetUserOptions.AddRange(
                        await SearchUsersAsync(model, model.TargetUserNameSearch, model.TargetUserEmailSearch));
                }

                if (model.SelectedOldUserId.HasValue)
                {
                    model.OldUser = await GetUserAsync(model, model.SelectedOldUserId.Value);
                }

                if (model.SelectedTargetUserId.HasValue)
                {
                    model.TargetUser = await GetUserAsync(model, model.SelectedTargetUserId.Value);
                }

                if (model.IsMerging && model is { SelectedOldUserId: not null, SelectedTargetUserId: not null })
                {
                    Response<User> mergedUserResponse = await Client.MergeUsersAsync(model.SelectedOldUserId.Value, model.SelectedTargetUserId.Value);
                    if (mergedUserResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
                    {
                        model.MergedUser = mergedUserResponse.Result;
                        ModelState.Clear();
                        model.SelectedOldUserId = null;
                        model.SelectedTargetUserId = null;
                        model.OldUserNameSearch = null;
                        model.OldUserEmailSearch = null;
                        model.TargetUserNameSearch = null;
                        model.TargetUserEmailSearch = null;
                        await LoadApplications(model, model.MergedUser.Id, model.MergedUser);
                    }
                    else
                    {
                        model.ErrorMessages.Add(mergedUserResponse.Message);
                    }
                }
            }
            else
            {
                model.ErrorMessages.Add("Only an Administrator can merge users.");
            }
        }
        else
        {
            model.ErrorMessages.Add(userResponse.Message);
        }

        return model.ErrorMessages.Count > 0 ? View("~/Views/Shared/_Error.cshtml", model) : View(model);
    }

    private async Task<List<User>> SearchUsersAsync(MergeUsersModel model, string? name, string? email)
    {
        List<User> result = [];
        if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(email))
        {
            Response<IList<User>> response = await Client.GetUsersAsync(name, email);
            if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                result.AddRange(response.Result);
            }
            else
            {
                model.ErrorMessages.Add(response.Message);
            }
        }

        return result;
    }

    private async Task<User?> GetUserAsync(MergeUsersModel model, Guid userId)
    {
        User? result = null;
        Response<User> response = await Client.GetUserAsync(userId);
        if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            result = response.Result;
            await LoadApplications(model, userId, result);
        }
        else
        {
            model.ErrorMessages.Add(response.Message);
        }

        return result;
    }

    private async Task LoadApplications(MergeUsersModel model, Guid userId, User user)
    {
        Response<IList<Application>> response = await Client.GetApplicationsForUserAsync(userId);
        if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            user.Applications.AddRange(response.Result);
        }
        else
        {
            model.ErrorMessages.Add(response.Message);
        }
    }
}