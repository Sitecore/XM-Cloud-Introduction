using MvpSite.Rendering.Models;
using MvpSite.Rendering.ViewComponents;
using MvpSite.Rendering.ViewComponents.Admin;
using MvpSite.Rendering.ViewComponents.Any;
using MvpSite.Rendering.ViewComponents.Apply;
using MvpSite.Rendering.ViewComponents.Mvp;
using Sitecore.AspNetCore.SDK.RenderingEngine.Configuration;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;

namespace MvpSite.Rendering.Extensions;

public static class RenderingEngineOptionsExtensions
{
    public static RenderingEngineOptions AddFeatureBasicContent(this RenderingEngineOptions options)
    {
        options.AddModelBoundView<AnnouncementBar>("AnnouncementBar")
            .AddModelBoundView<HalfWidthBanner>("HalfWidthBanner")
            .AddModelBoundView<ContentList>("ContentList")
            .AddModelBoundView<RichTextContent>("RichTextContent")
            .AddModelBoundView<ImageTeaser>("ImageTeaser")
            .AddModelBoundView<VideoTeaser>("VideoTeaser")
            .AddModelBoundView<EmbedContent>("EmbedContent")
            .AddPartialView("ColumnContainer")
            .AddPartialView("PageOverviewPanel")
            .AddModelBoundView<HeroBig>("HeroBig")
            .AddModelBoundView<HeroMedium>("HeroMedium")
            .AddModelBoundView<HeroMediumWithLink>("HeroMediumWithLink");

        return options;
    }

    public static RenderingEngineOptions AddFeatureNavigation(this RenderingEngineOptions options)
    {
        options.AddModelBoundView<TopLinks>("TopLinks")
            .AddModelBoundView<MainNav>("MainNav")
            .AddModelBoundView<Footer>("Footer");
        return options;
    }

    public static RenderingEngineOptions AddFeaturePeople(this RenderingEngineOptions options)
    {
        options.AddViewComponent(DirectoryViewComponent.ViewComponentName);
        options.AddViewComponent(ProfileViewComponent.ViewComponentName);
        return options;
    }

    public static RenderingEngineOptions AddFeatureSocial(this RenderingEngineOptions options)
    {
        options.AddModelBoundView<Rss>("Rss");
        return options;
    }

    public static RenderingEngineOptions AddFeatureUser(this RenderingEngineOptions options)
    {
        options.AddPartialView("~/Views/Shared/Components/SignIn.cshtml");
        return options;
    }

    public static RenderingEngineOptions AddFeatureSelections(this RenderingEngineOptions options)
    {
        options.AddViewComponent(UsersOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(UserEditViewComponent.ViewComponentName);
        options.AddViewComponent(SystemRolesOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(RegionsOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(CountriesOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(ApplicationOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(MvpTypesOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(DashboardViewComponent.ViewComponentName);
        options.AddViewComponent(ApplicationFormViewComponent.ViewComponentName);
        options.AddViewComponent(MyDataEditViewComponent.ViewComponentName);
        options.AddViewComponent(MyProfilesFormViewComponent.ViewComponentName);
        options.AddViewComponent(ScoreCardsViewComponent.ViewComponentName);
        options.AddViewComponent(ScoreCardDetailViewComponent.ViewComponentName);
        options.AddViewComponent(AwardViewComponent.ViewComponentName);
        options.AddViewComponent(ApplicationCommentViewComponent.ViewComponentName);
        options.AddViewComponent(ApplicationReviewSettingsViewComponent.ViewComponentName);
        options.AddViewComponent(ContributionOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(SelectionOverviewViewComponent.ViewComponentName);
        options.AddViewComponent(MergeUsersViewComponent.ViewComponentName);
        options.AddViewComponent(MvpMentorDataViewComponent.ViewComponentName);
        options.AddViewComponent(MvpLicenseDownloadViewComponent.ViewComponentName);
        options.AddViewComponent(LicensesUploadViewComponent.ViewComponentName);
        return options;
    }
}