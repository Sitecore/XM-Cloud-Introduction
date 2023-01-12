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
    public class MvpTypesOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminMvpTypesOverview";

        public MvpTypesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            MvpTypesOverviewModel model = await ModelBinder.Bind<MvpTypesOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                Response<IList<MvpType>> mvpTypesResponse = await Client.GetMvpTypesAsync(model.Page, model.PageSize);
                if (mvpTypesResponse.StatusCode == HttpStatusCode.OK && mvpTypesResponse.Result != null)
                {
                    model.List.AddRange(mvpTypesResponse.Result);
                }
            }

            return View(model);
        }

        private static void GenerateFakeDataForEdit(MvpTypesOverviewModel model)
        {
            model.List.Add(new MvpType(1)
            {
                Name = "Lorem"
            });
            model.List.Add(new MvpType(2)
            {
                Name = "Ipsum"
            });
            model.List.Add(new MvpType(3)
            {
                Name = "Dolor"
            });
            model.List.Add(new MvpType(4)
            {
                Name = "Sid"
            });
            model.List.Add(new MvpType(5)
            {
                Name = "Amet"
            });
        }
    }
}
