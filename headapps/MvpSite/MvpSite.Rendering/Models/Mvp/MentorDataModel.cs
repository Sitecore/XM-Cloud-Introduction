using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Mvp;

public class MentorDataModel : BaseModel
{
    public const int LargeTextFieldLength = 2000;

    public TextField? TitleLabel { get; set; }

    [StringLength(LargeTextFieldLength)]
    public TextField? MentorDescriptionLabel { get; set; }

    public TextField? IsMentorLabel { get; set; }

    public TextField? IsOpenToNewMenteesLabel { get; set; }

    public TextField? SubmitLabel { get; set; }

    public TextField? SuccessLabel { get; set; }

    public HyperLinkField? SuccessLink { get; set; }

    public bool IsMvp { get; set; }

    [FromForm(Name = $"{nameof(MentorDataModel)}.{nameof(IsEdit)}")]
    public bool IsEdit { get; set; }

    [FromForm(Name = $"{nameof(MentorDataModel)}.{nameof(MentorId)}")]
    public Guid? MentorId { get; set; }

    [FromForm(Name = $"{nameof(MentorDataModel)}.{nameof(IsMentor)}")]
    public bool IsMentor { get; set; }

    [FromForm(Name = $"{nameof(MentorDataModel)}.{nameof(IsOpenToNewMentees)}")]
    public bool IsOpenToNewMentees { get; set; }

    [FromForm(Name = $"{nameof(MentorDataModel)}.{nameof(MentorDescription)}")]
    public string? MentorDescription { get; set; }
}