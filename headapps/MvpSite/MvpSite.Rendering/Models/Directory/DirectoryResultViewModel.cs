using Mvp.Selections.Api.Model;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Extensions;

namespace MvpSite.Rendering.Models.Directory;

public class DirectoryResultViewModel
{
    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Country { get; set; }

    public Uri? Image { get; set; }

    public string? Year { get; set; }

    public Uri? ProfileUri { get; set; }

    public static DirectoryResultViewModel FromMvpProfile(MvpProfile profile, Uri? pageUri)
    {
        DirectoryResultViewModel result = new();
        Title last = profile.Titles.OrderByDescending(t => t.Application.Selection.Year).First();

        result.Name = profile.Name;
        result.Type = last.MvpType.Name;
        result.Country = profile.Country?.Name;
        result.Image = profile.ImageUri.AddGravatarSizing("250") ?? new Uri("/images/mvp-base-user-grey.png", UriKind.Relative);
        result.Year = last.Application.Selection.Year.ToString();
        if (pageUri != null && !string.IsNullOrWhiteSpace(result.Name))
        {
            result.ProfileUri = pageUri.AddQueryString("id", profile.Id.ToString("N"));
        }

        return result;
    }
}