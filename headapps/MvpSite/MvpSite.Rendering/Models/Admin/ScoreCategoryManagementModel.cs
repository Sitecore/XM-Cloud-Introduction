using Mvp.Selections.Domain;

namespace MvpSite.Rendering.Models.Admin;

public class ScoreCategoryManagementModel : BaseModel
{
    public List<Selection> Selections { get; } = [];

    public Guid SelectedSelectionId { get; set; } = Guid.Empty;

    public List<MvpType> MvpTypes { get; } = [];

    public short SelectedMvpTypeId { get; set; }

    public List<ScoreCategory> ScoreCategories { get; } = [];

    public List<CategoryEntry> CategoryEntries { get; } = [];

    public string? Command { get; set; }

    public Guid CategoryId { get; set; } = Guid.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public decimal CategoryWeight { get; set; }

    public int CategorySortRank { get; set; }

    public Guid? ParentCategoryId { get; set; }

    public string NewCategoryName { get; set; } = string.Empty;

    public decimal NewCategoryWeight { get; set; } = 1;

    public int NewCategorySortRank { get; set; }

    public Guid? NewParentCategoryId { get; set; }

    public string? SuccessMessage { get; set; }

    public bool HasSelectedScope => SelectedSelectionId != Guid.Empty && SelectedMvpTypeId > 0;

    public IEnumerable<CategoryEntry> ParentOptionsFor(ScoreCategory? category = null)
    {
        HashSet<Guid> excludedIds = [];
        if (category is not null)
        {
            ExcludeCategoryAndDescendants(category, excludedIds);
        }

        return CategoryEntries.Where(entry => !excludedIds.Contains(entry.Category.Id));
    }

    private static void ExcludeCategoryAndDescendants(ScoreCategory category, ISet<Guid> excludedIds)
    {
        if (!excludedIds.Add(category.Id))
        {
            return;
        }

        foreach (ScoreCategory child in category.SubCategories)
        {
            ExcludeCategoryAndDescendants(child, excludedIds);
        }
    }

    public sealed record CategoryEntry(ScoreCategory Category, int Depth, Guid? ParentCategoryId);
}
