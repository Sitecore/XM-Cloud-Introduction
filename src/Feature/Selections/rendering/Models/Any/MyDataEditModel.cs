using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Any
{
    public class MyDataEditModel : BaseModel
    {
        public TextField TitleLabel { get; set; }

        public bool IsEdit { get; set; } = false;

        public TextField NameLabel { get; set; }

        [Required]
        public string Name { get; set; }

        public TextField EmailLabel { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public TextField CountryLabel { get; set; }

        [Required]
        public short CountryId { get; set; }

        public List<Country> Countries { get; init; } = new ();

        public TextField ImageTypeLabel { get; set; }

        [Required]
        public ImageType ImageType { get; set; }

        public List<Consent> Consents { get; init; } = new ();

        public TextField SubmitLabel { get; set; }

        public TextField SuccessLabel { get; set; }

        public HyperLinkField SuccessLink { get; set; }
    }
}
