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

        public TextField LastModifiedTableHeader { get; set; }
    }
}
