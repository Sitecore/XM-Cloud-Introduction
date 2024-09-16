using System;
using System.Collections.Generic;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class ApplicationReviewSettingsModel : BaseModel
    {
        public TextField TitleLabel { get; set; }

        public TextField ApplicationDataLabel { get; set; }

        public TextField ApplicationIdLabel { get; set; }

        public TextField ApplicationApplicantNameLabel { get; set; }

        public TextField ApplicationSelectionYearLabel { get; set; }

        public TextField ApplicationMvpTypeNameLabel { get; set; }

        public TextField ApplicationCountryNameLabel { get; set; }

        public TextField ReviewersLabel { get; set; }

        public TextField ReviewerRemoveText { get; set; }

        public TextField AddReviewerLabel { get; set; }

        public TextField AddReviewerTitleLabel { get; set; }

        public TextField AddReviewerCancelLabel { get; set; }

        public TextField AddReviewerSubmitLabel { get; set; }

        public TextField AddReviewerUserEmailLabel { get; set; }

        public List<string> AddReviewerUserEmails { get; set; } = [];

        public Guid? RemoveReviewerUserId { get; set; }

        public TextField ConfirmDeleteLabelFormat { get; set; }

        public Guid Id { get; set; }

        public Application Application { get; set; }

        public List<User> Reviewers { get; set; } = [];
    }
}
