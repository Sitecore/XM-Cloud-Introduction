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
                        ExecuteProfileStep(model);
                        break;
                    case ApplicationStep.Objectives:
                        await ExecuteObjectivesStep(model);
                        break;
                    case ApplicationStep.Contributions:
                        await ExecuteContributionsStep(model);
                        break;
                    case ApplicationStep.Confirmation:
                        await ExecuteConfirmationStep(model);
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
                    ApplicationStep.Submitted => View("SubmittedStep", model),
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
                        Uri = new Uri("https://www.google.com")
                    }
                }
            };

            Product product1 = new (1)
            {
                Name = "Lorem Product"
            };
            Product product2 = new (2)
            {
                Name = "Ipsum Product"
            };

            MvpType mvpType1 = new (1)
            {
                Name = "Lorem MVP"
            };

            model.CurrentApplication = new Application(Guid.NewGuid())
            {
                Contributions = new List<Contribution>
                {
                    new (Guid.NewGuid())
                    {
                        Date = DateTime.UtcNow,
                        Name = "Lorem Link",
                        Description = "This would be a description of the link submitted as a contribution.",
                        Uri = new Uri("https://www.google.com"),
                        Type = ContributionType.Other,
                        RelatedProducts = new List<Product>
                        {
                            product1,
                            product2
                        }
                    }
                },
                ModifiedOn = DateTime.UtcNow,
                MvpType = mvpType1
            };

            model.MvpTypes.Add(mvpType1);
            model.MvpTypes.Add(new MvpType(2)
            {
                Name = "Ipsum MVP"
            });

            model.Products.Add(product1);
            model.Products.Add(product2);

            model.CurrentSelection = new Selection(Guid.NewGuid())
            {
                Year = (short)DateTime.UtcNow.Year
            };
        }

        private static void ExecuteProfileStep(ApplicationFormModel model)
        {
            if (model.IsNavigation.HasValue && model.IsNavigation.Value)
            {
                model.NextStep = ApplicationStep.Profile;
            }
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
            if (userResponse.StatusCode == HttpStatusCode.OK && userResponse.Result != null && userResponse.Result.Country != null)
            {
                model.CurrentUser = userResponse.Result;
            }
            else
            {
                model.ErrorMessages.Add("Your User doesn't have a Country set.");
                model.CurrentStep = ApplicationStep.Error;
                model.NextStep = ApplicationStep.Error;
            }
        }

        private async Task EstablishApplication(ApplicationFormModel model)
        {
            if (model.CurrentSelection != null && model.CurrentUser != null)
            {
                await LoadOpenCurrentApplication(model);
                if (model.CurrentApplication != null)
                {
                    model.MvpTypeId = model.CurrentApplication.MvpType.Id;
                    if (string.IsNullOrWhiteSpace(model.Eligibility))
                    {
                        model.Eligibility = model.CurrentApplication.Eligibility;
                        model.Objectives = model.CurrentApplication.Objectives;
                        model.Mentors = model.CurrentApplication.Mentor;

                        if (!model.IsNavigation.HasValue)
                        {
                            model.CurrentStep = ApplicationStep.Objectives;
                            model.NextStep = ApplicationStep.Objectives;
                        }
                    }

                    if (model.CurrentApplication.Contributions.Count > 0 && !model.IsNavigation.HasValue)
                    {
                        model.CurrentStep = ApplicationStep.Contributions;
                        model.NextStep = ApplicationStep.Contributions;
                    }
                }
                else
                {
                    await LoadSubmittedCurrentApplication(model);
                    if (model.CurrentApplication != null)
                    {
                        model.CurrentStep = ApplicationStep.Submitted;
                        model.NextStep = ApplicationStep.Submitted;
                    }
                }
            }
        }

        private async Task ExecuteConsentStep(ApplicationFormModel model)
        {
            if (model.IsNavigation.HasValue && !model.IsNavigation.Value && !model.IsConsentGiven)
            {
                ModelState.AddModelError(string.Empty, "Consent is required.");
                model.NextStep = ApplicationStep.Consent;
            }
            else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.IsConsentGiven)
            {
                Response<IList<Consent>> consentsResponse = await Client.GetConsentsAsync();
                if (consentsResponse.StatusCode == HttpStatusCode.OK &&
                    (consentsResponse.Result?.All(c => c.Type != ConsentType.PersonalInformation) ?? false))
                {
                    Consent consent = new (Guid.Empty)
                    {
                        Type = ConsentType.PersonalInformation
                    };
                    Response<Consent> consentResponse = await Client.GiveConsentAsync(consent);
                    if (consentResponse.StatusCode == HttpStatusCode.OK && consentResponse.Result != null)
                    {
                        await LoadMvpTypes(model);
                        model.NextStep = ApplicationStep.MvpType;
                    }
                    else
                    {
                        model.ErrorMessages.Add("Something went wrong while giving Consent.");
                        model.NextStep = ApplicationStep.Error;
                    }
                }
                else if (consentsResponse.StatusCode == HttpStatusCode.OK &&
                         (consentsResponse.Result?.Any(c => c.Type == ConsentType.PersonalInformation) ?? false))
                {
                    await LoadMvpTypes(model);
                    model.NextStep = ApplicationStep.MvpType;
                }
                else
                {
                    model.NextStep = ApplicationStep.Error;
                }
            }
            else if ((model.IsNavigation.HasValue && model.IsNavigation.Value) || !model.IsNavigation.HasValue)
            {
                model.NextStep = ApplicationStep.Consent;
            }
            else
            {
                model.NextStep = ApplicationStep.Error;
            }
        }

        private async Task ExecuteMvpTypeStep(ApplicationFormModel model)
        {
            if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.MvpTypeId > 0)
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
                        model.NextStep = ApplicationStep.Profile;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving your application. Please try again later or contact the MVP Program if the problem persists.");
                        model.NextStep = ApplicationStep.MvpType;
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
                        model.NextStep = ApplicationStep.Profile;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving your application. Please try again later or contact the MVP Program if the problem persists.");
                        model.NextStep = ApplicationStep.MvpType;
                    }
                }
            }
            else if ((model.IsNavigation.HasValue && model.IsNavigation.Value) || !model.IsNavigation.HasValue)
            {
                await LoadMvpTypes(model);
                model.NextStep = ApplicationStep.MvpType;
            }
            else if (model.IsNavigation.HasValue && !model.IsNavigation.Value)
            {
                await LoadMvpTypes(model);
                ModelState.AddModelError(string.Empty, "MVP Category must be selected.");
                model.NextStep = ApplicationStep.MvpType;
            }
        }

        private async Task ExecuteObjectivesStep(ApplicationFormModel model)
        {
            if (model.IsNavigation.HasValue && !model.IsNavigation.Value && !string.IsNullOrWhiteSpace(model.Eligibility) && !string.IsNullOrWhiteSpace(model.Objectives))
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
                    await LoadProducts(model);
                    model.NextStep = ApplicationStep.Contributions;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving your application. Please try again later or contact the MVP Program if the problem persists.");
                    model.NextStep = ApplicationStep.Objectives;
                }
            }
            else if ((model.IsNavigation.HasValue && model.IsNavigation.Value) || !model.IsNavigation.HasValue)
            {
                model.NextStep = ApplicationStep.Objectives;
            }
            else if (model.IsNavigation.HasValue && !model.IsNavigation.Value)
            {
                ModelState.AddModelError(string.Empty, "You must fill out the required fields.");
                model.NextStep = ApplicationStep.Objectives;
            }
        }

        private async Task ExecuteContributionsStep(ApplicationFormModel model)
        {
            if ((model.IsNavigation.HasValue && model.IsNavigation.Value) || !model.IsNavigation.HasValue)
            {
                await LoadProducts(model);
                model.NextStep = ApplicationStep.Contributions;
            }
            else if (
                model.IsNavigation.HasValue &&
                !model.IsNavigation.Value &&
                model.ContributionDate.HasValue &&
                !string.IsNullOrWhiteSpace(model.ContributionName) &&
                (string.IsNullOrWhiteSpace(model.ContributionLink?.OriginalString) || Uri.IsWellFormedUriString(model.ContributionLink?.OriginalString, UriKind.Absolute)))
            {
                Contribution contribution = new (Guid.Empty)
                {
                    Date = model.ContributionDate.Value,
                    Name = model.ContributionName,
                    Description = model.ContributionDescription,
                    Uri = model.ContributionLink,
                    Type = model.ContributionType
                };
                foreach (int productId in model.ContributionProductIds ?? new List<int>())
                {
                    contribution.RelatedProducts.Add(new Product(productId));
                }

                Response<Contribution> contributionResponse = await Client.AddContributionAsync(model.CurrentApplication.Id, contribution);
                if (contributionResponse.StatusCode == HttpStatusCode.OK && contributionResponse.Result != null)
                {
                    model.CurrentApplication.Contributions.Add(contributionResponse.Result);
                }

                model.ContributionDate = null;
                model.ContributionName = null;
                model.ContributionDescription = null;
                model.ContributionLink = null;
                model.ContributionType = ContributionType.Other;
                model.ContributionProductIds = new List<int>();
                await LoadProducts(model);
                model.NextStep = ApplicationStep.Contributions;
            }
            else if (
                model.IsNavigation.HasValue &&
                !model.IsNavigation.Value &&
                model.DeleteContributionId != null)
            {
                Response<bool> deleteResponse = await Client.RemoveContributionAsync(model.CurrentApplication.Id, model.DeleteContributionId.Value);
                if (deleteResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    Contribution contribution = model.CurrentApplication.Contributions.Single(c => c.Id == model.DeleteContributionId);
                    model.CurrentApplication.Contributions.Remove(contribution);
                    await LoadProducts(model);
                    model.NextStep = ApplicationStep.Contributions;
                }
                else
                {
                    model.NextStep = ApplicationStep.Error;
                }
            }
            else if (model.IsNavigation.HasValue && !model.IsNavigation.Value)
            {
                await LoadProducts(model);
                ModelState.AddModelError(string.Empty, "Your contribution is missing mandatory fields or has an invalid link.");
                model.NextStep = ApplicationStep.Contributions;
            }
        }

        private async Task ExecuteConfirmationStep(ApplicationFormModel model)
        {
            if (model.IsNavigation.HasValue && model.IsNavigation.Value)
            {
                model.NextStep = ApplicationStep.Confirmation;
            }
            else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.UnderstandsReviewConsent && model.UnderstandsProgramAgreement && model.IsComplete)
            {
                Application updateApplication = new (model.CurrentApplication.Id)
                {
                    Status = ApplicationStatus.Submitted
                };
                Response<Application> applicationResponse = await Client.UpdateApplicationAsync(updateApplication);
                if (applicationResponse.StatusCode == HttpStatusCode.OK && applicationResponse.Result != null)
                {
                    model.CurrentApplication = applicationResponse.Result;
                    model.NextStep = ApplicationStep.Submitted;
                }
                else
                {
                    model.NextStep = ApplicationStep.Error;
                }
            }
            else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && (!model.UnderstandsReviewConsent || !model.UnderstandsProgramAgreement || !model.IsComplete))
            {
                ModelState.AddModelError(string.Empty, "You must agree to all of the above to complete and submit your application.");
                model.NextStep = ApplicationStep.Confirmation;
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

        private async Task LoadProducts(ApplicationFormModel model)
        {
            Response<IList<Product>> productsResponse = await Client.GetProductsAsync(1, short.MaxValue);
            if (productsResponse.StatusCode == HttpStatusCode.OK && productsResponse.Result != null)
            {
                model.Products.AddRange(productsResponse.Result);
            }
        }

        private async Task LoadOpenCurrentApplication(ApplicationFormModel model)
        {
            Response<IList<Application>> applicationsResponse =
                await Client.GetApplicationsAsync(model.CurrentUser.Id, ApplicationStatus.Open);
            if (applicationsResponse.StatusCode == HttpStatusCode.OK && applicationsResponse.Result != null)
            {
                model.CurrentApplication =
                    applicationsResponse.Result.FirstOrDefault(a => a.Selection.Id == model.CurrentSelection.Id);
            }
        }

        private async Task LoadSubmittedCurrentApplication(ApplicationFormModel model)
        {
            Response<IList<Application>> applicationsResponse =
                await Client.GetApplicationsAsync(model.CurrentUser.Id, ApplicationStatus.Submitted);
            if (applicationsResponse.StatusCode == HttpStatusCode.OK && applicationsResponse.Result != null)
            {
                model.CurrentApplication =
                    applicationsResponse.Result.FirstOrDefault(a => a.Selection.Id == model.CurrentSelection.Id);
            }
        }
    }
}
