using System;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.People.Models.Profile
{
    public class ProfileViewModel : BaseModel
    {
        public Guid? Id { get; set; }

        public MvpProfile Mvp { get; set; }

        public TextField TitleLabel { get; set; }

        public TextField TitlesLabel { get; set; }

        public static Uri GenerateImageUri(Title title)
        {
            return new Uri($"/images/mvp-logos/{title.Application.Selection.Year}-{title.MvpType.Name}.png", UriKind.Relative);
        }
    }
}
