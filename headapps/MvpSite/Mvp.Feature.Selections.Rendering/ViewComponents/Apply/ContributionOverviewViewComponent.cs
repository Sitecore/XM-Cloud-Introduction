using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Apply;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Apply
{
    [ViewComponent(Name = ViewComponentName)]
    public class ContributionOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "ApplyContributionOverview";

        public ContributionOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            ContributionOverviewModel model = await ModelBinder.Bind<ContributionOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                if (model.TogglePublicContributionId.HasValue)
                {
                    await TogglePublic(model);
                }

                await LoadContributions(model);
            }

            return model.ErrorMessages.Count > 0 ? View("~/Views/Shared/_Error.cshtml", model) : View(model);
        }

        private static void GenerateFakeDataForEdit(ContributionOverviewModel model)
        {
            model.List.Add(new Contribution(Guid.NewGuid())
            {
                Date = DateTime.Today,
                Name = "Lorem",
                Type = ContributionType.Other,
                IsPublic = true,
                Uri = new Uri("https://www.google.com")
            });

            model.List.Add(new Contribution(Guid.NewGuid())
            {
                Date = DateTime.Today.AddDays(1),
                Name = "Ipsum",
                Type = ContributionType.Community,
                IsPublic = false
            });
        }

        private async Task LoadContributions(ContributionOverviewModel model)
        {
            Response<User> currentUserResponse = await Client.GetCurrentUserAsync();
            if (currentUserResponse.StatusCode == HttpStatusCode.OK && currentUserResponse.Result != null)
            {
                Response<IList<Contribution>> contributionsResponse =
                    await Client.GetContributionsForUserAsync(currentUserResponse.Result.Id, null, model.Page, model.PageSize);
                if (contributionsResponse.StatusCode == HttpStatusCode.OK && contributionsResponse.Result != null)
                {
                    model.List.AddRange(contributionsResponse.Result);
                }
                else
                {
                    model.ErrorMessages.Add(contributionsResponse.Message);
                }
            }
            else
            {
                model.ErrorMessages.Add(currentUserResponse.Message);
            }
        }

        private async Task TogglePublic(ContributionOverviewModel model)
        {
            Response<Contribution> response =
                await Client.TogglePublicContributionAsync(model.TogglePublicContributionId!.Value);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                model.TogglePublicContributionId = null;
            }
            else
            {
                model.ErrorMessages.Add(response.Message);
            }
        }
    }
}
