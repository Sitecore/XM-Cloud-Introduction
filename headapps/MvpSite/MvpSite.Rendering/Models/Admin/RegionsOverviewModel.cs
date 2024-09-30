using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin;

public class RegionsOverviewModel : ListModel<Region>
{
    public TextField? TitleLabel { get; set; }

    public TextField? NameTableHeader { get; set; }

    public TextField? CountryCountTableHeader { get; set; }
}