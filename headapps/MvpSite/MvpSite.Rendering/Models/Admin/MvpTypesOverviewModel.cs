using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin;

public class MvpTypesOverviewModel : ListModel<MvpType>
{
    public TextField? TitleLabel { get; set; }

    public TextField? NameTableHeader { get; set; }
}