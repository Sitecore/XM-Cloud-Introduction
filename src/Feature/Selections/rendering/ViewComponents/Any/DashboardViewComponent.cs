using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Mvp.Feature.Selections.Models.Any;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Sitecore.AspNet.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Any
{
    [ViewComponent(Name = ViewComponentName)]
    public class DashboardViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AnyDashboard";

        public DashboardViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            IViewComponentResult result = new ContentViewComponentResult(string.Empty);
            DashboardModel model = await ModelBinder.Bind<DashboardModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                Response<User> userResponse = await Client.GetCurrentUserAsync();
                if (userResponse.StatusCode == HttpStatusCode.OK)
                {
                    model.CurrentUser = userResponse.Result;
                }

                Response<Selection> selectionResponse = await Client.GetCurrentSelectionAsync();
                if (selectionResponse.StatusCode == HttpStatusCode.OK)
                {
                    model.CurrentSelection = selectionResponse.Result;
                }

                result = model.CurrentUser != null ?
                    View(model) :
                    View("SignIn", model);
            }

            return result;
        }

        private static void GenerateFakeDataForEdit(DashboardModel model)
        {
            model.CurrentUser = new User(Guid.NewGuid())
            {
                Name = "Fake User"
            };
            model.CurrentSelection = new Selection(Guid.NewGuid())
            {
                ApplicationsActive = true,
                ApplicationsStart = DateTime.Today.AddDays(-2),
                ApplicationsEnd = DateTime.Today.AddDays(2),
                ReviewsActive = true,
                ReviewsStart = DateTime.Today.AddDays(-2),
                ReviewsEnd = DateTime.Today.AddDays(2),
                Year = (short)DateTime.Today.Year
            };
        }
    }
}
