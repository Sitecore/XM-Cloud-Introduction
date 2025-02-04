using Mvp.Selections.Domain;

namespace MvpSite.Rendering.Models.Admin;

public class MergeUsersModel : BaseModel
{
    public List<User> OldUserOptions { get; init; } = [];

    public List<User> TargetUserOptions { get; init; } = [];

    public Guid? SelectedOldUserId { get; set; }

    public Guid? SelectedTargetUserId { get; set; }

    public User? OldUser { get; set; }

    public User? TargetUser { get; set; }

    public string? OldUserNameSearch { get; set; }

    public string? OldUserEmailSearch { get; set; }

    public string? TargetUserNameSearch { get; set; }

    public string? TargetUserEmailSearch { get; set; }

    public bool IsMerging { get; set; }

    public User? MergedUser { get; set; }
}