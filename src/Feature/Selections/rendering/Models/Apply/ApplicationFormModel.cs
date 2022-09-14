using System;
using System.Collections.Generic;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Apply
{
    public class ApplicationFormModel : BaseModel
    {
        public Application CurrentApplication { get; set; }

        public Selection CurrentSelection { get; set; }

        public User CurrentUser { get; set; }

        public ApplicationStep CurrentStep { get; set; } = ApplicationStep.Consent;

        public ApplicationStep NextStep { get; set; } = ApplicationStep.Consent;

        public bool IsBack { get; set; } = false;

        public TextField ConsentStepLabel { get; set; }

        public TextField MypTypeStepLabel { get; set; }

        public TextField ProfileStepLabel { get; set; }

        public TextField ObjectivesStepLabel { get; set; }

        public TextField ContributionsStepLabel { get; set; }

        public TextField ConfirmationStepLabel { get; set; }

        public TextField NextLabel { get; set; }

        public TextField PreviousLabel { get; set; }

        public TextField InactiveMessageLabel { get; set; }

        public TextField ErrorMessageLabel { get; set; }

        public TextField ConsentTitleLabel { get; set; }

        public TextField ConsentLabel { get; set; }

        public TextField StartLabel { get; set; }

        public RichTextField ConsentInformation { get; set; }

        public bool IsConsentGiven { get; set; } = false;

        public TextField MvpTypeTitleLabel { get; set; }

        public TextField MvpTypeLabel { get; set; }

        public short MvpTypeId { get; set; }

        public List<MvpType> MvpTypes { get; } = new ();

        public TextField ProfileTitleLabel { get; set; }

        public HyperLinkField MyUserLink { get; set; }

        public HyperLinkField OktaLink { get; set; }

        public TextField ObjectivesTitleLabel { get; set; }

        public TextField EligibilityLabel { get; set; }

        public string Eligibility { get; set; }

        public TextField ObjectivesLabel { get; set; }

        public string Objectives { get; set; }

        public TextField MentorsLabel { get; set; }

        public string Mentors { get; set; }

        public TextField ContributionsTitleLabel { get; set; }

        public TextField ContributionNameLabel { get; set; }

        public string ContributionName { get; set; }

        public TextField ContributionDescriptionLabel { get; set; }

        public string ContributionDescription { get; set; }

        public TextField ContributionLinkLabel { get; set; }

        public Uri ContributionLink { get; set; }

        public TextField ContributionDateLabel { get; set; }

        public DateTime? ContributionDate { get; set; }

        public TextField ContributionTypeLabel { get; set; }

        public ApplicationLinkType ContributionType { get; set; }

        public TextField ContributionProductsLabel { get; set; }

        public List<int> ContributionProductIds { get; set; }

        public List<Product> Products { get; set; }

        public TextField AddLabel { get; set; }

        public TextField ConfirmationTitleLabel { get; set; }

        public TextField ReviewConsentLabel { get; set; }

        public bool UnderstandsReviewConsent { get; set; } = false;

        public TextField ProgramAgreementWarningLabel { get; set; }

        public bool UnderstandsProgramAgreement { get; set; } = false;

        public TextField CompleteLabel { get; set; }

        public bool IsComplete { get; set; } = false;
    }
}
