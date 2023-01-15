using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Admin;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Mvp.Selections.Domain.Roles;
using Sitecore.AspNet.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Admin
{
    [ViewComponent(Name = ViewComponentName)]
    public class SystemRolesOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminSystemRolesOverview";

        public SystemRolesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            IViewComponentResult result = null;
            SystemRolesOverviewModel model = await ModelBinder.Bind<SystemRolesOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
                result = View(model);
            }
            else if (model.RemoveSystemRoleId != null && !model.RemoveConfirmed)
            {
                Response<SystemRole> roleResponse = await Client.GetSystemRoleAsync(model.RemoveSystemRoleId.Value);
                if (roleResponse.StatusCode == HttpStatusCode.OK && roleResponse.Result != null)
                {
                    model.RemoveSystemRole = roleResponse.Result;
                    result = View("Confirm", model);
                }
                else
                {
                    model.ErrorMessage = roleResponse.Message;
                }
            }
            else if (model.RemoveSystemRoleId != null && model.RemoveConfirmed)
            {
                Response<bool> removeResponse = await Client.RemoveRoleAsync(model.RemoveSystemRoleId.Value);
                if (removeResponse.Result)
                {
                    await LoadSystemRoles(model);
                    result = View(model);
                }
                else
                {
                    model.ErrorMessage = removeResponse.Message;
                }
            }
            else
            {
                await LoadSystemRoles(model);
                result = View(model);
            }

            return string.IsNullOrWhiteSpace(model.ErrorMessage) ?
                result :
                View("Error", model);
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
            if (rolesResponse.StatusCode == HttpStatusCode.OK && rolesResponse.Result != null)
            {
                model.List.AddRange(rolesResponse.Result);
            }
            else
            {
                model.ErrorMessage = rolesResponse.Message;
            }
        }
    }
}
