using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class CountriesOverviewModel : ListModel<Country>
    {
        public TextField TitleLabel { get; set; }

        public TextField NameTableHeader { get; set; }
    }
}
