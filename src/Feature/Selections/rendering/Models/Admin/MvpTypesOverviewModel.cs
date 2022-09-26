using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class MvpTypesOverviewModel : ListModel<MvpType>
    {
        public TextField TitleLabel { get; set; }

        public TextField NameTableHeader { get; set; }
    }
}
