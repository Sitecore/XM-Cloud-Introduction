using System.Net;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Profile;

public class ProfileViewModel
    : BaseViewModel
{
    public Guid? Id { get; set; }

    public MvpProfile? Mvp { get; set; }

    public TextField? TitleLabel { get; set; }

    public TextField? TitlesLabel { get; set; }

    public TextField? TimelineLabel { get; set; }

    public RichTextField? NotFoundText { get; set; }

    public static Uri GenerateImageUri(Title title)
    {
        return new Uri($"/images/mvp-logos/{title.Application.Selection.Year}-{WebUtility.UrlEncode(title.MvpType.Name)}.png", UriKind.Relative);
    }

    public List<TimelineEventViewModel> GetTimelineEvents()
    {
        List<TimelineEventViewModel> result = [];
        if (Mvp != null)
        {
            result.AddRange(Mvp.Titles.Select(title => new TimelineEventViewModel
            {
                Date = new DateTime(title.Application.Selection.Year, 1, 31),
                Title = $"{title.MvpType.Name} {title.Application.Selection.Year}",
                Description = $"![{title.MvpType.Name} {title.Application.Selection.Year}]({GenerateImageUri(title)} \"{title.MvpType.Name} {title.Application.Selection.Year}\")"
            }));
            result.AddRange(Mvp.PublicContributions.Select(contribution => new TimelineEventViewModel
            {
                Date = contribution.Date,
                Title = contribution.Name,
                Description = $"{contribution.Description[..(contribution.Description.Length > 500 ? 500 : contribution.Description.Length)]}{(contribution.Description.Length > 500 ? "..." : string.Empty)}",
                Uri = contribution.Uri,
                ContributionType = contribution.Type,
                RelatedProducts = contribution.RelatedProducts.Select(rp => rp.Name).ToList()
            }));
        }

        return result;
    }
}