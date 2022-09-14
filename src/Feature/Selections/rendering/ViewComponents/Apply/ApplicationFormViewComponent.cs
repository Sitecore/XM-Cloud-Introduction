using System;
using System.Collections.Generic;
using System.Linq;
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
            IViewComponentResult result;
            ApplicationFormModel model = await ModelBinder.Bind<ApplicationFormModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
                result = View("ExperienceEditor", model);
            }
            else
            {
                await EstablishSelection(model);
                await EstablishUser(model);
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
                        model.NextStep = model.IsBack ? ApplicationStep.Profile : ApplicationStep.Objectives;
                        break;
                    case ApplicationStep.Objectives:
                        await ExecuteObjectivesStep(model);
                        break;
                    case ApplicationStep.Contributions:
                        break;
                    case ApplicationStep.Confirmation:
                        break;
                }

                result = model.NextStep switch
                {
                    ApplicationStep.Consent => View("ConsentStep", model),
                    ApplicationStep.MvpType => View("MvpTypeStep", model),
                    ApplicationStep.Profile => View("ProfileStep", model),
                    ApplicationStep.Objectives => View("ObjectivesStep", model),
                    ApplicationStep.Contributions => View("ContributionsStep", model),
                    ApplicationStep.Confirmation => View("ConfirmationStep", model),
                    ApplicationStep.Inactive => View("Inactive", model),
                    _ => View(model)
                };
            }

            return result;
        }

        private static void GenerateFakeDataForEdit(ApplicationFormModel model)
        {
            model.CurrentUser = new User(Guid.NewGuid())
            {
                Name = "Lorem",
                Email = "ipsum@dolor.sid",
                Country = new Country(1)
                {
                    Name = "Country A"
                },
                Links =
                {
                    new ProfileLink(Guid.NewGuid())
                    {
                        Type = ProfileLinkType.Twitter,
                        Name = "Twitter",
                        Uri = new Uri("https://www.twitter.com")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Type = ProfileLinkType.LinkedIn,
                        Name = "LinkedIn",
                        Uri = new Uri("https://www.linkedin.com")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Type = ProfileLinkType.Blog,
                        Name = "My Blog",
                        Uri = new Uri("https://www.myblog.com")
                    }
                }
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

        private async Task EstablishUser(ApplicationFormModel model)
        {
            Response<User> userResponse = await Client.GetCurrentUserAsync();
            if (userResponse.StatusCode == HttpStatusCode.OK && userResponse.Result != null)
            {
                model.CurrentUser = userResponse.Result;
            }
            else
            {
                model.NextStep = ApplicationStep.Error;
            }
        }

        private async Task EstablishApplication(ApplicationFormModel model)
        {
            if (model.CurrentSelection != null && model.CurrentUser != null)
            {
                Response<IList<Application>> applicationsResponse =
                    await Client.GetApplicationsAsync(model.CurrentUser.Id, ApplicationStatus.Open);
                if (applicationsResponse.StatusCode == HttpStatusCode.OK && applicationsResponse.Result != null)
                {
                    model.CurrentApplication =
                        applicationsResponse.Result.FirstOrDefault(a => a.Selection.Id == model.CurrentSelection.Id);
                    if (model.CurrentApplication != null)
                    {
                        model.MvpTypeId = model.CurrentApplication.MvpType.Id;
                        model.Eligibility = model.CurrentApplication.Eligibility;
                        model.Objectives = model.CurrentApplication.Objectives;
                        model.Mentors = model.CurrentApplication.Mentor;
                    }
                }
            }
        }

        private async Task ExecuteConsentStep(ApplicationFormModel model)
        {
            if (!model.IsBack && model.IsConsentGiven)
            {
                if (model.CurrentUser != null && model.CurrentUser.Consents.All(c => c.Type != ConsentType.PersonalInformation))
                {
                    Consent consent = new (Guid.Empty)
                    {
                        User = new User(model.CurrentUser.Id),
                        Type = ConsentType.PersonalInformation
                    };

                    // TODO [ILs] Add consent
                }

                await LoadMvpTypes(model);
                model.NextStep = ApplicationStep.MvpType;
            }
            else if (model.IsBack)
            {
                model.NextStep = ApplicationStep.Consent;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Consent is required.");
                model.NextStep = ApplicationStep.Consent;
            }
        }

        private async Task ExecuteMvpTypeStep(ApplicationFormModel model)
        {
            if (!model.IsBack && model.MvpTypeId > 0)
            {
                if (model.CurrentApplication != null)
                {
                    Application updateApplication = new (model.CurrentApplication.Id)
                    {
                        MvpType = new MvpType(model.MvpTypeId)
                    };
                    Response<Application> applicationResponse =
                        await Client.UpdateApplicationAsync(updateApplication);
                    if (applicationResponse.StatusCode == HttpStatusCode.OK && applicationResponse.Result != null)
                    {
                        model.CurrentApplication = applicationResponse.Result;
                    }
                }
                else
                {
                    Application newApplication = new (Guid.Empty)
                    {
                        Country = model.CurrentUser.Country,
                        Selection = model.CurrentSelection,
                        Applicant = model.CurrentUser,
                        Status = ApplicationStatus.Open,
                        MvpType = new MvpType(model.MvpTypeId)
                    };
                    Response<Application> applicationResponse =
                        await Client.AddApplicationAsync(model.CurrentSelection.Id, newApplication);
                    if (applicationResponse.StatusCode == HttpStatusCode.OK && applicationResponse.Result != null)
                    {
                        model.CurrentApplication = applicationResponse.Result;
                    }
                }

                model.NextStep = ApplicationStep.Profile;
            }
            else if (model.IsBack)
            {
                await LoadMvpTypes(model);
                model.NextStep = ApplicationStep.MvpType;
            }
            else
            {
                await LoadMvpTypes(model);
                ModelState.AddModelError(string.Empty, "MVP Category must be selected.");
                model.NextStep = ApplicationStep.MvpType;
            }
        }

        private async Task ExecuteObjectivesStep(ApplicationFormModel model)
        {
            if (!model.IsBack && !string.IsNullOrWhiteSpace(model.Eligibility) && !string.IsNullOrWhiteSpace(model.Objectives))
            {
                Application updateApplication = new (model.CurrentApplication.Id)
                {
                    Eligibility = model.Eligibility,
                    Objectives = model.Objectives,
                    Mentor = model.Mentors
                };
                Response<Application> applicationResponse =
                    await Client.UpdateApplicationAsync(updateApplication);
                if (applicationResponse.StatusCode == HttpStatusCode.OK && applicationResponse.Result != null)
                {
                    model.CurrentApplication = applicationResponse.Result;
                }
            }
            else if (model.IsBack)
            {
                model.NextStep = ApplicationStep.Objectives;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "You must fill out the required fields.");
                model.NextStep = ApplicationStep.Contributions;
            }
        }

        private async Task LoadMvpTypes(ApplicationFormModel model)
        {
            Response<IList<MvpType>> mvpTypesResponse = await Client.GetMvpTypesAsync(1, short.MaxValue);
            if (mvpTypesResponse.StatusCode == HttpStatusCode.OK && mvpTypesResponse.Result != null)
            {
                model.MvpTypes.AddRange(mvpTypesResponse.Result);
            }
        }
    }
}
