using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Any;

public class DashboardModel : BaseModel
{
    public HyperLinkField? AdminScoreCardsLink { get; set; }

    public HyperLinkField? AdminUsersOverviewLink { get; set; }

    public HyperLinkField? AdminRolesOverviewLink { get; set; }

    public HyperLinkField? AdminSelectionsOverviewLink { get; set; }

    public HyperLinkField? AdminApplicationsOverviewLink { get; set; }

    public HyperLinkField? AdminRegionsOverviewLink { get; set; }

    public HyperLinkField? AdminCountriesOverviewLink { get; set; }

    public HyperLinkField? AdminLicensesUploadLink { get; set; }

    public HyperLinkField? ReviewApplicationsOverviewLink { get; set; }

    public HyperLinkField? MyUserOverviewLink { get; set; }

    public HyperLinkField? MyApplicationsOverviewLink { get; set; }

    public HyperLinkField? MyContributionsOverviewLink { get; set; }

    public HyperLinkField? MyCurrentApplicationLink { get; set; }

    public HyperLinkField? MvpEmailContactLink { get; set; }

    public TextField? MySectionLabel { get; set; }

    public TextField? AdminSectionLabel { get; set; }

    public TextField? ReviewSectionLabel { get; set; }

    public TextField? ApplicationsOpenLabel { get; set; }

    public TextField? ApplicationsClosedLabel { get; set; }

    public TextField? ReviewsOpenLabel { get; set; }

    public TextField? ReviewsClosedLabel { get; set; }

    public User? CurrentUser { get; set; }

    public Selection? CurrentSelection { get; set; }

    public TextField? SignInText { get; set; }
}