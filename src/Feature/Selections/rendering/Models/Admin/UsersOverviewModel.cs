using System.Collections.Generic;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class UsersOverviewModel : ListModel
    {
        public TextField TitleLabel { get; set; }

        public TextField IdentifierTableHeader { get; set; }

        public TextField OperationsTableHeader { get; set; }

        public List<User> Users { get; } = new ();
    }
}
