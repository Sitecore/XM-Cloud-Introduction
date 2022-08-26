using System.Collections.Generic;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class RegionsOverviewModel : ListModel
    {
        public TextField TitleLabel { get; set; }

        public TextField NameTableHeader { get; set; }

        public TextField CountryCountTableHeader { get; set; }

        public TextField OperationsTableHeader { get; set; }

        public List<Region> Regions { get; } = new ();
    }
}
