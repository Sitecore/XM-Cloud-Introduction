using System;
using System.Collections.Generic;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class ApplicationsOverviewModel : ListModel<Application>
    {
        public TextField TitleLabel { get; set; }

        public TextField SelectionTableHeader { get; set; }

        public TextField CountryTableHeader { get; set; }

        public TextField ApplicantTableHeader { get; set; }

        public TextField StatusTableHeader { get; set; }

        public TextField ContributionsTableHeader { get; set; }

        public TextField LastModifiedTableHeader { get; set; }

        public Guid? RemoveApplicationId { get; set; }

        public Application RemoveApplication { get; set; }

        public TextField ConfirmMessageLabelFormat { get; set; }

        public TextField ConfirmLabel { get; set; }

        public bool RemoveConfirmed { get; set; } = false;

        public TextField ReviewLinkFormat { get; set; }

        public ApplicationOverviewFilter Filter { get; set; } = new ();

        public List<Country> Countries { get; set; } = new ();

        public List<Selection> Selections { get; set; } = new ();

        public HyperLinkField ReviewSettingsLink { get; set; }

        public class ApplicationOverviewFilter
        {
            public Guid? SelectionId { get; set; }

            public short? CountryId { get; set; }

            public string ApplicantName { get; set; }

            public ApplicationStatus? Status { get; set; }
        }
    }
}
