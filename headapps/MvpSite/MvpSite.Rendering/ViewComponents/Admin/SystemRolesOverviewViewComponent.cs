using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Mvp.Selections.Domain.Roles;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class SystemRolesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminSystemRolesOverview";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult? result = null;
        SystemRolesOverviewModel model = await ModelBinder.Bind<SystemRolesOverviewModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
            result = View(model);
        }
        else if (model is { RemoveSystemRoleId: not null, RemoveConfirmed: false })
        {
            Response<SystemRole> roleResponse = await Client.GetSystemRoleAsync(model.RemoveSystemRoleId.Value);
            if (roleResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.RemoveSystemRole = roleResponse.Result;
                result = View("Confirm", model);
            }
            else
            {
                model.ErrorMessages.Add(roleResponse.Message);
            }
        }
        else if (model is { RemoveSystemRoleId: not null, RemoveConfirmed: true })
        {
            Response<bool> removeResponse = await Client.RemoveRoleAsync(model.RemoveSystemRoleId.Value);
            if (removeResponse.Result)
            {
                await LoadSystemRoles(model);
                result = View(model);
            }
            else
            {
                model.ErrorMessages.Add(removeResponse.Message);
            }
        }
        else
        {
            await LoadSystemRoles(model);
            result = View(model);
        }

        return model.ErrorMessages.Count > 0 || result == null
            ? View("~/Views/Shared/_Error.cshtml", model)
            : result;
    }

    private static void GenerateFakeDataForEdit(SystemRolesOverviewModel model)
    {
        model.List.Add(new SystemRole(Guid.NewGuid())
        {
            Name = "Lorem",
            Rights = Right.Admin
        });
        model.List.Add(new SystemRole(Guid.NewGuid())
        {
            Name = "Dolor",
            Rights = Right.Apply
        });
        model.List.Add(new SystemRole(Guid.NewGuid())
        {
            Name = "Sid",
            Rights = Right.Review
        });
        model.List.Add(new SystemRole(Guid.NewGuid())
        {
            Name = "Amet",
            Rights = Right.Admin | Right.Apply | Right.Review
        });
    }

    private async Task LoadSystemRoles(SystemRolesOverviewModel model)
    {
        Response<IList<SystemRole>> rolesResponse = await Client.GetSystemRolesAsync(model.Page, model.PageSize);
        if (rolesResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.List.AddRange(rolesResponse.Result);
        }
        else
        {
            model.ErrorMessages.Add(rolesResponse.Message);
        }
    }
}