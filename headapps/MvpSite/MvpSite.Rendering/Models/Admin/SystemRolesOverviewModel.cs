using Mvp.Selections.Domain.Roles;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin;

public class SystemRolesOverviewModel : ListModel<SystemRole>
{
    public TextField? TitleLabel { get; set; }

    public TextField? NameTableHeader { get; set; }

    public TextField? RightsTableHeader { get; set; }

    public HyperLinkField? NewSystemRoleLink { get; set; }

    public Guid? RemoveSystemRoleId { get; set; }

    public SystemRole? RemoveSystemRole { get; set; }

    public bool RemoveConfirmed { get; set; } = false;

    public TextField? ConfirmMessageLabelFormat { get; set; }

    public TextField? ConfirmLabel { get; set; }
}