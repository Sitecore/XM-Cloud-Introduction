using System;
using System.Collections.Generic;
using Mvp.Feature.Selections.Models.Filters;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class ApplicationsOverviewModel : ListModel<Application>
    {
        public ApplicationsOverviewModel()
        {
            Filter = new ApplicationFilter($"{nameof(Filter)}");
            base.Filter = Filter;
        }

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

        public List<Country> Countries { get; set; } = new ();

        public List<Selection> Selections { get; set; } = new ();

        public HyperLinkField ReviewSettingsLink { get; set; }

        public new ApplicationFilter Filter { get; set; }
    }
}
