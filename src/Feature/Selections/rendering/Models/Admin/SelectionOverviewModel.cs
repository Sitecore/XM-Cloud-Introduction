using System;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class SelectionOverviewModel : ListModel<Selection>
    {
        public TextField TitleLabel { get; set; }

        public TextField ApplicationsLabel { get; set; }

        public TextField ReviewsLabel { get; set; }

        public TextField ActivateLabel { get; set; }

        public TextField DeactivateLabel { get; set; }

        public TextField ClearOverrideLabel { get; set; }

        public TextField YearTableHeader { get; set; }

        public TextField ApplicationsActiveTableHeader { get; set; }

        public TextField ApplicationsStartTableHeader { get; set; }

        public TextField ApplicationsEndTableHeader { get; set; }

        public TextField AreApplicationsOpenTableHeader { get; set; }

        public TextField ReviewsActiveTableHeader { get; set; }

        public TextField ReviewsStartTableHeader { get; set; }

        public TextField ReviewsEndTableHeader { get; set; }

        public TextField AreReviewsOpenTableHeader { get; set; }

        public Guid? ActivateApplicationsSelectionId { get; set; }

        public Guid? DeactivateApplicationsSelectionId { get; set; }

        public Guid? ClearOverrideApplicationsSelectionId { get; set; }

        public Guid? ActivateReviewsSelectionId { get; set; }

        public Guid? DeactivateReviewsSelectionId { get; set; }

        public Guid? ClearOverrideReviewsSelectionId { get; set; }
    }
}
