using System;
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
    public class SystemRolesOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminSystemRolesOverview";

        public SystemRolesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            SystemRolesOverviewModel model = await ModelBinder.Bind<SystemRolesOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                Response<IList<SystemRole>> rolesResponse = await Client.GetSystemRolesAsync(model.Page, model.PageSize);
                if (rolesResponse.StatusCode == HttpStatusCode.OK && rolesResponse.Result != null)
                {
                    model.List.AddRange(rolesResponse.Result);
                }
            }

            return View(model);
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
    }
}
