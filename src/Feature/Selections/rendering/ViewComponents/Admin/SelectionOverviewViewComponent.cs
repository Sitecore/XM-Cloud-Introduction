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
    public class SelectionOverviewViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminSelectionOverview";

        public SelectionOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            SelectionOverviewModel model = await ModelBinder.Bind<SelectionOverviewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                await ExecuteCommands(model);
                await LoadSelections(model);
            }

            return model.ErrorMessages.Count > 0 ? View("~/Views/Shared/_Error.cshtml", model) : View(model);
        }

        private static void GenerateFakeDataForEdit(SelectionOverviewModel model)
        {
            model.List.Add(new Selection(Guid.NewGuid())
            {
                Year = (short)DateTime.Now.Year,
                ApplicationsActive = true,
                ApplicationsStart = DateTime.Now.AddMonths(-1),
                ApplicationsEnd = DateTime.Now.AddMonths(1),
                ReviewsActive = false,
                ReviewsStart = DateTime.Now.AddMonths(1).AddDays(1),
                ReviewsEnd = DateTime.Now.AddMonths(2)
            });
        }

        private async Task LoadSelections(SelectionOverviewModel model)
        {
            Response<IList<Selection>> selectionsResponse = await Client.GetSelectionsAsync(model.Page, model.PageSize);
            if (selectionsResponse.StatusCode == HttpStatusCode.OK && selectionsResponse.Result != null)
            {
                model.List.AddRange(selectionsResponse.Result);
            }
        }

        private async Task ExecuteCommands(SelectionOverviewModel model)
        {
            if (model.ActivateApplicationsSelectionId.HasValue)
            {
                Response<Selection> updateResponse =
                    await Client.PatchSelectionAsync(
                        model.ActivateApplicationsSelectionId.Value,
                        new { ApplicationsActive = true });
                if (updateResponse.StatusCode != HttpStatusCode.OK)
                {
                    model.ErrorMessages.Add(updateResponse.Message);
                }
            }
            else if (model.DeactivateApplicationsSelectionId.HasValue)
            {
                Response<Selection> updateResponse =
                    await Client.PatchSelectionAsync(
                        model.DeactivateApplicationsSelectionId.Value,
                        new { ApplicationsActive = false });
                if (updateResponse.StatusCode != HttpStatusCode.OK)
                {
                    model.ErrorMessages.Add(updateResponse.Message);
                }
            }
            else if (model.ClearOverrideApplicationsSelectionId.HasValue)
            {
                Response<Selection> updateResponse =
                    await Client.PatchSelectionAsync(
                        model.ClearOverrideApplicationsSelectionId.Value,
                        new { ApplicationsActive = default(bool?) });
                if (updateResponse.StatusCode != HttpStatusCode.OK)
                {
                    model.ErrorMessages.Add(updateResponse.Message);
                }
            }
            else if (model.ActivateReviewsSelectionId.HasValue)
            {
                Response<Selection> updateResponse =
                    await Client.PatchSelectionAsync(
                        model.ActivateReviewsSelectionId.Value,
                        new { ReviewsActive = true });
                if (updateResponse.StatusCode != HttpStatusCode.OK)
                {
                    model.ErrorMessages.Add(updateResponse.Message);
                }
            }
            else if (model.DeactivateReviewsSelectionId.HasValue)
            {
                Response<Selection> updateResponse =
                    await Client.PatchSelectionAsync(
                        model.DeactivateReviewsSelectionId.Value,
                        new { ReviewsActive = false });
                if (updateResponse.StatusCode != HttpStatusCode.OK)
                {
                    model.ErrorMessages.Add(updateResponse.Message);
                }
            }
            else if (model.ClearOverrideReviewsSelectionId.HasValue)
            {
                Response<Selection> updateResponse =
                    await Client.PatchSelectionAsync(
                        model.ClearOverrideReviewsSelectionId.Value,
                        new { ReviewsActive = default(bool?) });
                if (updateResponse.StatusCode != HttpStatusCode.OK)
                {
                    model.ErrorMessages.Add(updateResponse.Message);
                }
            }
        }
    }
}
