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
    public class ApplicationOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminApplicationsOverview";

        public ApplicationOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            ApplicationsOverviewModel model = await ModelBinder.Bind<ApplicationsOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                Response<IList<Application>> response = await Client.GetApplicationsAsync(model.Page, model.PageSize);
                if (response.StatusCode == HttpStatusCode.OK && response.Result != null)
                {
                    model.List.AddRange(response.Result);
                }
            }

            return View(model);
        }

        private void GenerateFakeDataForEdit(ApplicationsOverviewModel model)
        {
            model.List.Add(new Application(Guid.NewGuid())
            {
                Selection = new Selection(Guid.NewGuid())
                {
                    Year = (short)DateTime.Today.Year
                },
                Country = new Country(1)
                {
                    Name = "Country A"
                },
                Applicant = new User(Guid.NewGuid())
                {
                    Name = "John Doe"
                },
                Status = ApplicationStatus.Open,
                ModifiedOn = DateTime.Now
            });
            model.List.Add(new Application(Guid.NewGuid())
            {
                Selection = new Selection(Guid.NewGuid())
                {
                    Year = (short)DateTime.Today.Year
                },
                Country = new Country(2)
                {
                    Name = "Country B"
                },
                Applicant = new User(Guid.NewGuid())
                {
                    Name = "Jane Doe"
                },
                Status = ApplicationStatus.Submitted,
                ModifiedOn = DateTime.Today.AddDays(-1)
            });
        }
    }
}
