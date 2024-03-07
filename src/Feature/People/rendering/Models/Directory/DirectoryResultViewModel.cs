using System;
using System.Linq;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Domain;

namespace Mvp.Feature.People.Models.Directory
{
    public class DirectoryResultViewModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Country { get; set; }

        public Uri Image { get; set; }

        public string Year { get; set; }

        public Uri ProfileUri { get; set; }

        public static DirectoryResultViewModel FromMvpProfile(MvpProfile profile, Uri baseUri)
        {
            DirectoryResultViewModel result = new ();
            Title last = profile.Titles.OrderByDescending(t => t.Application.Selection.Year).First();

            result.Name = profile.Name;
            result.Type = last.MvpType.Name;
            result.Country = profile.Country?.Name;
            result.Image = profile.ImageUri ?? new Uri("/images/mvp-base-user-grey.png", UriKind.Relative);
            if (result.Image.IsAbsoluteUri && result.Image.Host == "www.gravatar.com")
            {
                result.Image = new Uri(result.Image.OriginalString + "?s=250&d=https%3A%2F%2Fmvp.sitecore.net%2Fimages%2Fmvp-base-user-grey.png");
            }
            result.Year = last.Application.Selection.Year.ToString();
            if (baseUri != null && Uri.TryCreate(baseUri, profile.Id.ToString("N"), out Uri profileUri))
            {
                result.ProfileUri = profileUri;
            }

            return result;
        }
    }
}
