using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class SystemRolesOverviewModel : ListModel<SystemRole>
    {
        public TextField TitleLabel { get; set; }

        public TextField NameTableHeader { get; set; }

        public TextField RightsTableHeader { get; set; }
    }
}
