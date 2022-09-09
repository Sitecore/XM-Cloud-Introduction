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
    public class UsersOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminUsersOverview";

        public UsersOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            UsersOverviewModel model = await ModelBinder.Bind<UsersOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                Response<IList<User>> usersResponse = await Client.GetUsersAsync(model.Page, model.PageSize);
                if (usersResponse.StatusCode == HttpStatusCode.OK && usersResponse.Result != null)
                {
                    model.List.AddRange(usersResponse.Result);
                }
            }

            return View(model);
        }

        private static void GenerateFakeDataForEdit(UsersOverviewModel model)
        {
            model.List.Add(new User(Guid.NewGuid())
            {
                Identifier = "LoremIpsum"
            });
            model.List.Add(new User(Guid.NewGuid())
            {
                Identifier = "DolorSid"
            });
            model.List.Add(new User(Guid.NewGuid())
            {
                Identifier = "AmetErgo"
            });
        }
    }
}
