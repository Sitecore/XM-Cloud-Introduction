using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class UserEditModel : BaseModel
    {
        [FromQuery(Name = "id")]
        public Guid Id { get; set; } = Guid.Empty;

        public TextField IdentifierLabel { get; set; }

        [Required]
        public string Identifier { get; set; }

        public TextField NameLabel { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public TextField EmailLabel { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public TextField CountryLabel { get; set; }

        [Required]
        public short CountryId { get; set; }

        public List<Country> Countries { get; } = new ();

        public TextField SubmitLabel { get; set; }

        public bool IsEdit { get; set; } = false;

        public TextField SuccessLabel { get; set; }

        public HyperLinkField OverviewLink { get; set; }
    }
}
