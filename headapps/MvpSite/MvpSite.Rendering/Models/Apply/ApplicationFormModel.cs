using System.ComponentModel.DataAnnotations;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Apply;

public class ApplicationFormModel : BaseModel
{
    public const int LargeTextFieldLength = 2000;

    public Application? CurrentApplication { get; set; }

    public Selection? CurrentSelection { get; set; }

    public User? CurrentUser { get; set; }

    public ApplicationStep CurrentStep { get; set; } = ApplicationStep.Consent;

    public ApplicationStep NextStep { get; set; } = ApplicationStep.Consent;

    public bool? IsNavigation { get; set; }

    public TextField? ConsentStepLabel { get; set; }

    public TextField? MypTypeStepLabel { get; set; }

    public TextField? ProfileStepLabel { get; set; }

    public TextField? ObjectivesStepLabel { get; set; }

    public TextField? ContributionsStepLabel { get; set; }

    public TextField? ConfirmationStepLabel { get; set; }

    public TextField? NextLabel { get; set; }

    public TextField? PreviousLabel { get; set; }

    public TextField? InactiveMessageLabel { get; set; }

    public TextField? ErrorMessageLabel { get; set; }

    public TextField? ConsentTitleLabel { get; set; }

    public TextField? ConsentLabel { get; set; }

    public TextField? StartLabel { get; set; }

    public RichTextField? ConsentInformation { get; set; }

    public bool IsConsentGiven { get; set; }

    public TextField? MvpTypeTitleLabel { get; set; }

    public TextField? MvpTypeHelpText { get; set; }

    public TextField? MvpTypeLabel { get; set; }

    public short MvpTypeId { get; set; }

    public TextField? ProfileTitleLabel { get; set; }

    public HyperLinkField? MyUserLink { get; set; }

    public TextField? ObjectivesTitleLabel { get; set; }

    public TextField? EligibilityLabel { get; set; }

    [StringLength(LargeTextFieldLength)]
    public string? Eligibility { get; set; }

    public TextField? ObjectivesLabel { get; set; }

    [StringLength(LargeTextFieldLength)]
    public string? Objectives { get; set; }

    public TextField? MentorsLabel { get; set; }

    public string? Mentors { get; set; }

    public TextField? ContributionsTitleLabel { get; set; }

    public TextField? ContributionsHelpText { get; set; }

    public TextField? ContributionNameLabel { get; set; }

    public string? ContributionName { get; set; }

    public TextField? ContributionDescriptionLabel { get; set; }

    [StringLength(LargeTextFieldLength)]
    public string? ContributionDescription { get; set; }

    public TextField? ContributionLinkLabel { get; set; }

    public Uri? ContributionLink { get; set; }

    public TextField? ContributionDateLabel { get; set; }

    public DateTime? ContributionDate { get; set; }

    public TextField? ContributionTypeLabel { get; set; }

    public ContributionType ContributionType { get; set; }

    public TextField? ContributionProductsLabel { get; set; }

    public List<int> ContributionProductIds { get; set; } = [];

    public TextField? PublicContributionLabel { get; set; }

    public bool ContributionIsPublic { get; set; }

    public List<Product> Products { get; } = [];

    public TextField? AddLabel { get; set; }

    public TextField? UpdateLabel { get; set; }

    public Guid? DeleteContributionId { get; set; }

    public Guid? UpdateContributionId { get; set; }

    public bool IsUpdate { get; set; }

    public TextField? ContributionConfirmDeleteLabel { get; set; }

    public TextField? ConfirmationTitleLabel { get; set; }

    public TextField? ReviewConsentLabel { get; set; }

    public bool UnderstandsReviewConsent { get; set; }

    public TextField? ProgramAgreementWarningLabel { get; set; }

    public bool UnderstandsProgramAgreement { get; set; }

    public TextField? CompleteLabel { get; set; }

    public bool IsComplete { get; set; }

    public TextField? SubmitLabel { get; set; }

    public TextField? SubmittedMessageLabelFormat { get; set; }

    public TextField? StepLabelFormat { get; set; }

    public TextField? RevokeLabel { get; set; }
}