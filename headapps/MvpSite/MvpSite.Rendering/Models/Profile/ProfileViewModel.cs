using System.ComponentModel.DataAnnotations;
using System.Net;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Profile;

public class ProfileViewModel
    : BaseViewModel
{
    public const int LargeTextFieldLength = 2000;

    public Guid? Id { get; set; }

    public MvpProfile? Mvp { get; set; }

    public TextField? TitleLabel { get; set; }

    public TextField? TitlesLabel { get; set; }

    public TextField? TimelineLabel { get; set; }

    public RichTextField? NotFoundText { get; set; }

    public TextField? MentorLabel { get; set; }

    public TextField? BecomeMenteeLabel { get; set; }

    public TextField? ContactLabel { get; set; }

    public RichTextField? ContactHelpText { get; set; }

    public TextField? ContactNameLabel { get; set; }

    public TextField? ContactEmailLabel { get; set; }

    public TextField? ContactWrongDataLabelFormat { get; set; }

    public HyperLinkField? ContactMyDataLink { get; set; }

    public TextField? ContactMessageLabel { get; set; }

    public TextField? ContactEmailConsentLabel { get; set; }

    [StringLength(LargeTextFieldLength)]
    public string? ContactMessage { get; set; }

    public bool ContactEmailConsent { get; set; }

    public TextField? ContactSendLabel { get; set; }

    public bool IsSending { get; set; }

    public bool IsSent { get; set; }

    public RichTextField? ContactSuccessText { get; set; }

    public User? CurrentUser { get; set; }

    public TextField? ContactMessageMandatoryLabel { get; set; }

    public TextField? ContactEmailConsentMandatoryLabel { get; set; }

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