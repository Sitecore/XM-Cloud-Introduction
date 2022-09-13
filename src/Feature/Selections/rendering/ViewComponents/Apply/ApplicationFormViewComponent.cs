using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Apply;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Sitecore.AspNet.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Apply
{
    [ViewComponent(Name = ViewComponentName)]
    public class ApplicationFormViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "ApplyApplicationForm";

        public ApplicationFormViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            ApplicationFormModel model = await ModelBinder.Bind<ApplicationFormModel>(ViewContext);
            await EstablishSelection(model);
            await EstablishApplication(model);
            switch (model.CurrentStep)
            {
                case ApplicationStep.Consent:
                    await ExecuteConsentStep(model);
                    break;
                case ApplicationStep.MvpType:
                    await ExecuteMvpTypeStep(model);
                    break;
                case ApplicationStep.Profile:
                    break;
                case ApplicationStep.Objectives:
                    break;
                case ApplicationStep.Contributions:
                    break;
                case ApplicationStep.Confirmation:
                    break;
            }

            return model.NextStep switch
            {
                ApplicationStep.Consent => View("ConsentStep", model),
                ApplicationStep.MvpType => View("MvpTypeStep", model),
                ApplicationStep.Profile => View("ProfileStep", model),
                ApplicationStep.Objectives => View("ObjectivesStep", model),
                ApplicationStep.Contributions => View("ContributionsStep", model),
                ApplicationStep.Confirmation => View("ConfirmationStep", model),
                _ => View(model)
            };
        }

        private async Task EstablishSelection(ApplicationFormModel model)
        {
            Response<Selection> selectionResponse = await Client.GetCurrentSelectionAsync();
            if (selectionResponse.StatusCode == HttpStatusCode.OK &&
                selectionResponse.Result != null &&
                (selectionResponse.Result.ApplicationsActive ??
                 (selectionResponse.Result.ApplicationsStart <= DateTime.UtcNow &&
                  selectionResponse.Result.ApplicationsEnd > DateTime.UtcNow)))
            {
                model.CurrentSelection = selectionResponse.Result;
            }
            else
            {
                model.CurrentStep = ApplicationStep.Inactive;
                model.NextStep = ApplicationStep.Inactive;
            }
        }

        private async Task EstablishApplication(ApplicationFormModel model)
        {
            if (model.CurrentSelection != null)
            {
                
            }
        }

        private async Task ExecuteConsentStep(ApplicationFormModel model)
        {
            if (model.IsConsentGiven)
            {
                model.NextStep = ApplicationStep.MvpType;
                Response<IList<MvpType>> mvpTypesResponse = await Client.GetMvpTypesAsync(1, short.MaxValue);
                if (mvpTypesResponse.StatusCode == HttpStatusCode.OK && mvpTypesResponse.Result != null)
                {
                    model.MvpTypes.AddRange(mvpTypesResponse.Result);
                }
            }
            else
            {
                model.NextStep = ApplicationStep.Consent;
                ModelState.AddModelError(string.Empty, "Consent is required.");
            }
        }

        private async Task ExecuteMvpTypeStep(ApplicationFormModel model)
        {
            if (model.MvpTypeId > 0)
            {

            }
            else
            {
                model.NextStep = ApplicationStep.MvpType;
                ModelState.AddModelError(string.Empty, "MVP Category must be selected.");
            }
        }
    }
}
