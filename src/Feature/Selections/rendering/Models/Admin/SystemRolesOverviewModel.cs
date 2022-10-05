using System;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class SystemRolesOverviewModel : ListModel<SystemRole>
    {
        public TextField TitleLabel { get; set; }

        public TextField NameTableHeader { get; set; }

        public TextField RightsTableHeader { get; set; }

        public HyperLinkField NewSystemRoleLink { get; set; }

        public Guid? RemoveSystemRoleId { get; set; }

        public SystemRole RemoveSystemRole { get; set; }

        public bool RemoveConfirmed { get; set; } = false;

        public TextField ConfirmMessageLabelFormat { get; set; }

        public TextField ConfirmLabel { get; set; }

        public string ErrorMessage { get; set; }
    }
}
