using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Configuration;
using MvpSite.Rendering.Models.Apply;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Apply;

[ViewComponent(Name = ViewComponentName)]
public class ApplicationFormViewComponent(
    IViewModelBinder modelBinder,
    MvpSelectionsApiClient client,
    IOptions<MvpSelectionsOptions> options)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "ApplyApplicationForm";

    private readonly MvpSelectionsOptions _options = options.Value;

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
                case ApplicationStep.Profile:
                    ExecuteProfileStep(model);
                    break;
                case ApplicationStep.MvpType:
                    await ExecuteMvpTypeStep(model);
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
                case ApplicationStep.Submitted:
                    await ExecuteSubmittedStep(model);
                    break;
            }

            result = model.NextStep switch
            {
                ApplicationStep.Consent => View("ConsentStep", model),
                ApplicationStep.Profile => View("ProfileStep", model),
                ApplicationStep.MvpType => View("MvpTypeStep", model),
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

        Product product1 = new(1)
        {
            Name = "Lorem Product"
        };
        Product product2 = new(2)
        {
            Name = "Ipsum Product"
        };

        MvpType mvpType1 = new(1)
        {
            Name = "Lorem MVP"
        };

        model.CurrentApplication = new Application(Guid.NewGuid())
        {
            Contributions =
            [
                new Contribution(Guid.NewGuid())
                {
                    Date = DateTime.UtcNow,
                    Name = "Lorem Link",
                    Description = "This would be a description of the link submitted as a contribution.",
                    Uri = new Uri("https://www.google.com"),
                    Type = ContributionType.Other,
                    RelatedProducts =
                    [
                        product1,
                        product2
                    ]
                }
            ],
            ModifiedOn = DateTime.UtcNow,
            MvpType = mvpType1
        };

        model.Products.Add(product1);
        model.Products.Add(product2);

        model.CurrentSelection = new Selection(Guid.NewGuid())
        {
            Year = (short)DateTime.UtcNow.Year,
            MvpTypes =
            [
                mvpType1,
                new MvpType(2)
                {
                    Name = "Ipsum MVP"
                }
            ]
        };
    }

    private async Task EstablishSelection(ApplicationFormModel model)
    {
        Response<Selection> selectionResponse = await Client.GetCurrentSelectionAsync();
        if (selectionResponse is { StatusCode: HttpStatusCode.OK, Result: not null } &&
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
        if (userResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.CurrentUser = userResponse.Result;
        }
        else
        {
            model.ErrorMessages.Add(userResponse.Message);
            model.CurrentStep = ApplicationStep.Error;
            model.NextStep = ApplicationStep.Error;
        }
    }

    private async Task EstablishApplication(ApplicationFormModel model)
    {
        if (model is { CurrentSelection: not null, CurrentUser: not null })
        {
            await LoadOpenCurrentApplication(model);
            if (model.CurrentApplication != null)
            {
                if (model.CurrentStep != ApplicationStep.MvpType || (model.IsNavigation ?? false))
                {
                    model.MvpTypeId = model.CurrentApplication.MvpType.Id;
                }

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
                Consent consent = new(Guid.Empty)
                {
                    Type = ConsentType.PersonalInformation
                };
                Response<Consent> consentResponse = await Client.GiveConsentAsync(consent);
                if (consentResponse is { StatusCode: HttpStatusCode.Created, Result: not null })
                {
                    model.NextStep = ApplicationStep.Profile;
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
                model.NextStep = ApplicationStep.Profile;
            }
            else
            {
                model.ErrorMessages.Add(consentsResponse.Message);
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

    private void ExecuteProfileStep(ApplicationFormModel model)
    {
        if (model.IsNavigation.HasValue && model.IsNavigation.Value)
        {
            model.NextStep = ApplicationStep.Profile;
        }
        else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.CurrentUser?.Country == null)
        {
            ModelState.AddModelError(string.Empty, "You must have a Country set on your profile. Edit your details to fix this issue.");
            model.NextStep = ApplicationStep.Profile;
        }
        else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.CurrentUser?.Country != null)
        {
            model.NextStep = ApplicationStep.MvpType;
        }
    }

    private async Task ExecuteMvpTypeStep(ApplicationFormModel model)
    {
        if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.MvpTypeId > 0)
        {
            if (model.CurrentApplication != null)
            {
                model.CurrentApplication.MvpType = new MvpType(model.MvpTypeId);
                Response<Application> applicationResponse =
                    await Client.UpdateApplicationAsync(model.CurrentApplication);
                if (applicationResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
                {
                    model.CurrentApplication = applicationResponse.Result;
                    model.NextStep = ApplicationStep.Objectives;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving your application. Please try again later or contact the MVP Program if the problem persists.");
                    model.NextStep = ApplicationStep.MvpType;
                }
            }
            else
            {
                Application newApplication = new(Guid.Empty)
                {
                    Country = model.CurrentUser?.Country!,
                    Selection = model.CurrentSelection!,
                    Applicant = model.CurrentUser!,
                    Status = ApplicationStatus.Open,
                    MvpType = new MvpType(model.MvpTypeId)
                };
                Response<Application> applicationResponse =
                    await Client.AddApplicationAsync(model.CurrentSelection!.Id, newApplication);
                if (applicationResponse is { StatusCode: HttpStatusCode.Created, Result: not null })
                {
                    model.CurrentApplication = applicationResponse.Result;
                    model.NextStep = ApplicationStep.Objectives;
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
            model.NextStep = ApplicationStep.MvpType;
        }
        else if (model.IsNavigation.HasValue && !model.IsNavigation.Value)
        {
            ModelState.AddModelError(string.Empty, "MVP Category must be selected.");
            model.NextStep = ApplicationStep.MvpType;
        }
    }

    private async Task ExecuteObjectivesStep(ApplicationFormModel model)
    {
        if (model.IsNavigation.HasValue && !model.IsNavigation.Value && !string.IsNullOrWhiteSpace(model.Eligibility) && !string.IsNullOrWhiteSpace(model.Objectives) && model.CurrentApplication != null)
        {
            model.CurrentApplication.Eligibility = model.Eligibility;
            model.CurrentApplication.Objectives = model.Objectives;
            model.CurrentApplication.Mentor = model.Mentors;
            Response<Application> applicationResponse =
                await Client.UpdateApplicationAsync(model.CurrentApplication);
            if (applicationResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
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
        else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model is { UpdateContributionId: not null, IsUpdate: false })
        {
            Contribution? editContribution =
                model.CurrentApplication?.Contributions.FirstOrDefault(c =>
                    c.Id == model.UpdateContributionId.Value);
            if (editContribution != null)
            {
                model.ContributionDate = editContribution.Date;
                model.ContributionName = editContribution.Name;
                model.ContributionDescription = editContribution.Description;
                model.ContributionLink = editContribution.Uri;
                model.ContributionType = editContribution.Type;
                model.ContributionIsPublic = editContribution.IsPublic;
                foreach (Product product in editContribution.RelatedProducts)
                {
                    model.ContributionProductIds.Add(product.Id);
                }
            }

            await LoadProducts(model);
            model.NextStep = ApplicationStep.Contributions;
        }
        else if (
            model.IsNavigation.HasValue
            && !model.IsNavigation.Value
            && model.ContributionDate.HasValue
            && !string.IsNullOrWhiteSpace(model.ContributionName)
            && (
                string.IsNullOrWhiteSpace(model.ContributionLink?.OriginalString)
                || Uri.IsWellFormedUriString(model.ContributionLink?.OriginalString, UriKind.Absolute))
            && model.ContributionDate != null
            && model.ContributionDate.Value >= model.CurrentSelection?.ApplicationsEnd.AddMonths(-_options.TimeFrameMonths)
            && model.ContributionDate.Value <= model.CurrentSelection.ApplicationsEnd)
        {
            if (model.CurrentApplication?.Contributions.Count >= Convert.ToInt32(model.MaxContributions?.Value))
            {
                model.NextStep = ApplicationStep.Contributions;
                await LoadProducts(model);
                ModelState.AddModelError(string.Empty, model.ContributionMinLimitMessage?.Value ?? "You have reached maximum number of contributions");
                return;
            }

            Contribution contribution = new(model.UpdateContributionId ?? Guid.Empty)
            {
                Date = model.ContributionDate.Value,
                Name = model.ContributionName,
                Description = model.ContributionDescription ?? string.Empty,
                Uri = model.ContributionLink,
                Type = model.ContributionType,
                IsPublic = model.ContributionIsPublic
            };
            foreach (int productId in model.ContributionProductIds)
            {
                contribution.RelatedProducts.Add(new Product(productId));
            }

            Response<Contribution> contributionResponse = model.UpdateContributionId.HasValue
                ? await Client.UpdateContributionAsync(model.UpdateContributionId.Value, contribution)
                : await Client.AddContributionAsync(model.CurrentApplication!.Id, contribution);
            if (contributionResponse.StatusCode is HttpStatusCode.Created or HttpStatusCode.OK && contributionResponse.Result != null)
            {
                if (model.UpdateContributionId.HasValue)
                {
                    Contribution? editContribution =
                        model.CurrentApplication?.Contributions.FirstOrDefault(c =>
                            c.Id == model.UpdateContributionId.Value);
                    if (editContribution != null)
                    {
                        model.CurrentApplication?.Contributions.Remove(editContribution);
                    }
                }

                model.UpdateContributionId = null;
                model.CurrentApplication?.Contributions.Add(contributionResponse.Result);
                model.ContributionDate = null;
                model.ContributionName = null;
                model.ContributionDescription = null;
                model.ContributionLink = null;
                model.ContributionType = ContributionType.Other;
                model.ContributionProductIds = [];
                model.ContributionIsPublic = false;
                ModelState.Clear();
            }
            else
            {
                model.ErrorMessages.Add(contributionResponse.Message);
                model.NextStep = ApplicationStep.Error;
            }

            await LoadProducts(model);
            model.NextStep = ApplicationStep.Contributions;
        }
        else if (
            model.IsNavigation.HasValue &&
            !model.IsNavigation.Value &&
            model.DeleteContributionId != null)
        {
            Response<bool> deleteResponse = await Client.RemoveContributionAsync(model.CurrentApplication!.Id, model.DeleteContributionId.Value);
            if (deleteResponse.StatusCode == HttpStatusCode.NoContent)
            {
                Contribution? contribution = model.CurrentApplication.Contributions.SingleOrDefault(c => c.Id == model.DeleteContributionId);
                if (contribution != null)
                {
                    model.CurrentApplication.Contributions.Remove(contribution);
                }

                await LoadProducts(model);
                model.NextStep = ApplicationStep.Contributions;
            }
            else
            {
                model.ErrorMessages.Add(deleteResponse.Message);
                model.NextStep = ApplicationStep.Error;
            }
        }
        else if (model.IsNavigation.HasValue && !model.IsNavigation.Value)
        {
            await LoadProducts(model);
            if (
                model.ContributionDate == null
                || model.ContributionDate.Value <= model.CurrentSelection?.ApplicationsEnd.AddMonths(-_options.TimeFrameMonths)
                || model.ContributionDate.Value >= model.CurrentSelection?.ApplicationsEnd)
            {
                ModelState.AddModelError(string.Empty, $"Your contribution date is not in the valid time frame ({model.CurrentSelection?.ApplicationsEnd.AddMonths(-_options.TimeFrameMonths):d} - {model.CurrentSelection?.ApplicationsEnd:d}).");
            }

            if (string.IsNullOrWhiteSpace(model.ContributionName))
            {
                ModelState.AddModelError(string.Empty, "Your contribution name is empty.");
            }

            if (
                !string.IsNullOrWhiteSpace(model.ContributionLink?.OriginalString)
                && !Uri.IsWellFormedUriString(model.ContributionLink?.OriginalString, UriKind.Absolute))
            {
                ModelState.AddModelError(string.Empty, "Your contribution link is invalid.");
            }

            model.NextStep = ApplicationStep.Contributions;
        }
    }

    private async Task ExecuteConfirmationStep(ApplicationFormModel model)
    {
        if (model.IsNavigation.HasValue && model.IsNavigation.Value)
        {
            if (model.CurrentApplication?.Contributions.Count < Convert.ToInt32(model.MinContributions?.Value))
            {
                ModelState.AddModelError(string.Empty, model.ContributionMinLimitMessage?.Value ?? "Minimum of 3 contributions required");
                model.NextStep = ApplicationStep.Contributions;
                return;
            }

            model.NextStep = ApplicationStep.Confirmation;
        }
        else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.UnderstandsReviewConsent && model is { UnderstandsProgramAgreement: true, IsComplete: true, CurrentApplication: not null })
        {
            model.CurrentApplication.Status = ApplicationStatus.Submitted;
            Response<Application> applicationResponse = await Client.UpdateApplicationAsync(model.CurrentApplication);
            if (applicationResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.CurrentApplication = applicationResponse.Result;
                model.NextStep = ApplicationStep.Submitted;
            }
            else
            {
                model.ErrorMessages.Add(applicationResponse.Message);
                model.NextStep = ApplicationStep.Error;
            }
        }
        else if (model.IsNavigation.HasValue && !model.IsNavigation.Value && (!model.UnderstandsReviewConsent || !model.UnderstandsProgramAgreement || !model.IsComplete))
        {
            ModelState.AddModelError(string.Empty, "You must agree to all of the above to complete and submit your application.");
            model.NextStep = ApplicationStep.Confirmation;
        }
    }

    private async Task ExecuteSubmittedStep(ApplicationFormModel model)
    {
        if (model.IsNavigation.HasValue && !model.IsNavigation.Value && model.CurrentApplication != null)
        {
            model.CurrentApplication.Status = ApplicationStatus.Open;
            Response<Application> applicationResponse = await Client.UpdateApplicationAsync(model.CurrentApplication);
            if (applicationResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.CurrentApplication = applicationResponse.Result;
                await LoadProducts(model);
                model.NextStep = ApplicationStep.Contributions;
            }
            else
            {
                model.ErrorMessages.Add(applicationResponse.Message);
                model.NextStep = ApplicationStep.Error;
            }
        }
    }

    private async Task LoadProducts(ApplicationFormModel model)
    {
        Response<IList<Product>> productsResponse = await Client.GetProductsAsync(1, short.MaxValue);
        if (productsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Products.AddRange(productsResponse.Result);
        }
    }

    private async Task LoadOpenCurrentApplication(ApplicationFormModel model)
    {
        Response<IList<Application>> applicationsResponse =
            await Client.GetApplicationsForUserAsync(model.CurrentUser!.Id, ApplicationStatus.Open);
        if (applicationsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.CurrentApplication =
                applicationsResponse.Result.FirstOrDefault(a => a.Selection.Id == model.CurrentSelection?.Id);
        }
    }

    private async Task LoadSubmittedCurrentApplication(ApplicationFormModel model)
    {
        Response<IList<Application>> applicationsResponse =
            await Client.GetApplicationsForUserAsync(model.CurrentUser!.Id, ApplicationStatus.Submitted);
        if (applicationsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.CurrentApplication =
                applicationsResponse.Result.FirstOrDefault(a => a.Selection.Id == model.CurrentSelection?.Id);
        }
    }
}