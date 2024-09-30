using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Apply;

public class ContributionOverviewModel : ListModel<Contribution>
{
    public TextField? TitleLabel { get; set; }

    public TextField? DateTableHeader { get; set; }

    public TextField? NameTableHeader { get; set; }

    public TextField? TypeTableHeader { get; set; }

    public TextField? UriTableHeader { get; set; }

    public TextField? IsPublicTableHeader { get; set; }

    public Guid? TogglePublicContributionId { get; set; }
}