using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Any
{
    public class MyDataEditModel : BaseModel
    {
        public TextField TitleLabel { get; set; }

        [FromForm(Name = $"{nameof(MyDataEditModel)}.{nameof(IsEdit)}")]
        public bool IsEdit { get; set; } = false;

        public TextField NameLabel { get; set; }

        [Required]
        [FromForm(Name = $"{nameof(MyDataEditModel)}.{nameof(Name)}")]
        public string Name { get; set; }

        public TextField EmailLabel { get; set; }

        [Required]
        [EmailAddress]
        [FromForm(Name = $"{nameof(MyDataEditModel)}.{nameof(Email)}")]
        public string Email { get; set; }

        public TextField CountryLabel { get; set; }

        [Required]
        [FromForm(Name = $"{nameof(MyDataEditModel)}.{nameof(CountryId)}")]
        public short CountryId { get; set; }

        public List<Country> Countries { get; init; } = new ();

        public TextField ImageTypeLabel { get; set; }

        [Required]
        [FromForm(Name = $"{nameof(MyDataEditModel)}.{nameof(ImageType)}")]
        public ImageType ImageType { get; set; }

        public Uri ImageUri { get; set; }

        public List<Consent> Consents { get; init; } = new ();

        public TextField SubmitLabel { get; set; }

        public TextField SuccessLabel { get; set; }

        public HyperLinkField SuccessLink { get; set; }
    }
}
