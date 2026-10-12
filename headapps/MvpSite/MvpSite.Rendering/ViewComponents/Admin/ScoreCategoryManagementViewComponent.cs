using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class ScoreCategoryManagementViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminScoreCategoryManagement";

    private const string CreateCommand = "Create";
    private const string UpdateCommand = "Update";
    private const string DeleteCommand = "Delete";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        ScoreCategoryManagementModel model = await ModelBinder.Bind<ScoreCategoryManagementModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
            PopulateCategoryEntries(model);
            return View(model);
        }

        await Task.WhenAll(LoadSelections(model), LoadMvpTypes(model));

        bool hasCommand = !string.IsNullOrWhiteSpace(model.Command);
        if (TryValidateScope(model, hasCommand))
        {
            await LoadScoreCategories(model);
            if (hasCommand && ModelState.IsValid && model.ErrorMessages.Count == 0)
            {
                await ExecuteCommand(model);
            }
        }

        PopulateCategoryEntries(model);
        return View(model);
    }

    private static void GenerateFakeDataForEdit(ScoreCategoryManagementModel model)
    {
        MvpType mvpType = new(1) { Name = "Technology" };
        Selection selection = new(Guid.NewGuid()) { Year = (short)DateTime.Now.Year };
        ScoreCategory parent = new(Guid.NewGuid())
        {
            Name = "Core Skills",
            Weight = 1,
            SortRank = 1,
            MvpType = mvpType,
            Selection = selection
        };
        ScoreCategory child = new(Guid.NewGuid())
        {
            Name = "Community Contributions",
            Weight = 0.5m,
            SortRank = 1,
            MvpType = mvpType,
            Selection = selection,
            ParentCategory = parent
        };
        parent.SubCategories.Add(child);

        model.SelectedSelectionId = selection.Id;
        model.SelectedMvpTypeId = mvpType.Id;
        model.Selections.Add(selection);
        model.MvpTypes.Add(mvpType);
        model.ScoreCategories.Add(parent);
    }

    private async Task LoadSelections(ScoreCategoryManagementModel model)
    {
        Response<IList<Selection>> response = await Client.GetSelectionsAsync(1, short.MaxValue);
        if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Selections.AddRange(response.Result);
        }
        else
        {
            AddError(model, response.Message, "Unable to load selections.");
        }
    }

    private async Task LoadMvpTypes(ScoreCategoryManagementModel model)
    {
        Response<IList<MvpType>> response = await Client.GetMvpTypesAsync(1, short.MaxValue);
        if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.MvpTypes.AddRange(response.Result);
        }
        else
        {
            AddError(model, response.Message, "Unable to load MVP types.");
        }
    }

    private bool TryValidateScope(ScoreCategoryManagementModel model, bool required)
    {
        bool isValid = true;
        if (!model.Selections.Any(selection => selection.Id == model.SelectedSelectionId))
        {
            if (required || model.SelectedSelectionId != Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.SelectedSelectionId), "Select a valid selection.");
            }

            isValid = false;
        }

        if (!model.MvpTypes.Any(mvpType => mvpType.Id == model.SelectedMvpTypeId))
        {
            if (required || model.SelectedMvpTypeId != 0)
            {
                ModelState.AddModelError(nameof(model.SelectedMvpTypeId), "Select a valid MVP type.");
            }

            isValid = false;
        }

        return isValid;
    }

    private async Task LoadScoreCategories(ScoreCategoryManagementModel model)
    {
        model.ScoreCategories.Clear();
        model.CategoryEntries.Clear();

        Response<IList<ScoreCategory>> response = await Client.GetScoreCategoriesAsync(
            model.SelectedSelectionId,
            model.SelectedMvpTypeId);
        if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.ScoreCategories.AddRange(response.Result);
        }
        else
        {
            AddError(model, response.Message, "Unable to load score categories.");
        }
    }

    private async Task ExecuteCommand(ScoreCategoryManagementModel model)
    {
        switch (model.Command)
        {
            case CreateCommand:
                await CreateCategory(model);
                break;
            case UpdateCommand:
                await UpdateCategory(model);
                break;
            case DeleteCommand:
                await DeleteCategory(model);
                break;
            default:
                ModelState.AddModelError(nameof(model.Command), "The requested category operation is not supported.");
                break;
        }
    }

    private async Task CreateCategory(ScoreCategoryManagementModel model)
    {
        if (string.IsNullOrWhiteSpace(model.NewCategoryName))
        {
            ModelState.AddModelError(nameof(model.NewCategoryName), "A category name is required.");
            return;
        }

        ScoreCategory? parent = null;
        if (model.NewParentCategoryId.HasValue)
        {
            parent = FindCategory(model.ScoreCategories, model.NewParentCategoryId.Value);
            if (parent is null)
            {
                ModelState.AddModelError(nameof(model.NewParentCategoryId), "Select a parent from this score category list.");
                return;
            }
        }

        ScoreCategory category = new(Guid.NewGuid())
        {
            Name = model.NewCategoryName.Trim(),
            Weight = model.NewCategoryWeight,
            SortRank = model.NewCategorySortRank,
            MvpType = new MvpType(model.SelectedMvpTypeId),
            Selection = new Selection(model.SelectedSelectionId),
            ParentCategory = parent is null ? null : new ScoreCategory(parent.Id)
        };

        Response<ScoreCategory> response = await Client.AddScoreCategoryAsync(
            model.SelectedSelectionId,
            model.SelectedMvpTypeId,
            category);
        if (response.StatusCode is HttpStatusCode.Created or HttpStatusCode.OK)
        {
            await CompleteCommand(model, "Score category created.");
        }
        else
        {
            AddError(model, response.Message, "Unable to create the score category.");
        }
    }

    private async Task UpdateCategory(ScoreCategoryManagementModel model)
    {
        if (string.IsNullOrWhiteSpace(model.CategoryName))
        {
            ModelState.AddModelError(nameof(model.CategoryName), "A category name is required.");
            return;
        }

        ScoreCategory? existing = FindCategory(model.ScoreCategories, model.CategoryId);
        if (existing is null)
        {
            ModelState.AddModelError(nameof(model.CategoryId), "The category is not part of the selected selection and MVP type.");
            return;
        }

        ScoreCategory? parent = null;
        if (model.ParentCategoryId.HasValue)
        {
            parent = FindCategory(model.ScoreCategories, model.ParentCategoryId.Value);
            if (parent is null)
            {
                ModelState.AddModelError(nameof(model.ParentCategoryId), "Select a parent from this score category list.");
                return;
            }

            if (parent.Id == existing.Id || IsDescendant(existing, parent.Id))
            {
                ModelState.AddModelError(nameof(model.ParentCategoryId), "A category cannot be moved beneath itself or one of its descendants.");
                return;
            }
        }

        ScoreCategory category = CopyCategory(
            existing,
            model.CategoryName.Trim(),
            model.CategoryWeight,
            model.CategorySortRank,
            parent?.Id,
            model.SelectedSelectionId,
            model.SelectedMvpTypeId);

        Response<ScoreCategory> response = await Client.UpdateScoreCategoryAsync(category);
        if (response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NoContent)
        {
            await CompleteCommand(model, "Score category updated.");
        }
        else
        {
            AddError(model, response.Message, "Unable to update the score category.");
        }
    }

    private async Task DeleteCategory(ScoreCategoryManagementModel model)
    {
        ScoreCategory? existing = FindCategory(model.ScoreCategories, model.CategoryId);
        if (existing is null)
        {
            ModelState.AddModelError(nameof(model.CategoryId), "The category is not part of the selected selection and MVP type.");
            return;
        }

        Response<bool> response = await Client.RemoveScoreCategoryAsync(existing.Id);
        if (response.StatusCode == HttpStatusCode.NoContent ||
            response is { StatusCode: HttpStatusCode.OK, Result: true })
        {
            await CompleteCommand(model, "Score category deleted.");
        }
        else
        {
            AddError(model, response.Message, "Unable to delete the score category.");
        }
    }

    private async Task CompleteCommand(ScoreCategoryManagementModel model, string message)
    {
        model.SuccessMessage = message;
        model.Command = null;
        model.CategoryId = Guid.Empty;
        model.CategoryName = string.Empty;
        model.CategoryWeight = 0;
        model.CategorySortRank = 0;
        model.ParentCategoryId = null;
        model.NewCategoryName = string.Empty;
        model.NewCategoryWeight = 1;
        model.NewCategorySortRank = 0;
        model.NewParentCategoryId = null;
        ModelState.Clear();
        await LoadScoreCategories(model);
    }

    private static ScoreCategory CopyCategory(
        ScoreCategory source,
        string name,
        decimal weight,
        int sortRank,
        Guid? parentId,
        Guid selectionId,
        short mvpTypeId)
    {
        return new ScoreCategory(source.Id)
        {
            Name = name,
            Weight = weight,
            SortRank = sortRank,
            Selection = new Selection(selectionId),
            MvpType = new MvpType(mvpTypeId),
            ParentCategory = parentId.HasValue ? new ScoreCategory(parentId.Value) : null,
            CalculationScore = CopyScore(source.CalculationScore),
            ScoreOptions = source.ScoreOptions.Select(CopyScore).ToList(),
            SubCategories = source.SubCategories
                .Select(child => CopyCategory(
                    child,
                    child.Name,
                    child.Weight,
                    child.SortRank,
                    source.Id,
                    selectionId,
                    mvpTypeId))
                .ToList()
        };
    }

    private static Score? CopyScore(Score? source)
    {
        return source is null
            ? null
            : new Score(source.Id)
            {
                Name = source.Name,
                Value = source.Value,
                SortRank = source.SortRank
            };
    }

    private static ScoreCategory? FindCategory(IEnumerable<ScoreCategory> categories, Guid id)
    {
        foreach (ScoreCategory category in categories)
        {
            if (category.Id == id)
            {
                return category;
            }

            ScoreCategory? match = FindCategory(category.SubCategories, id);
            if (match is not null)
            {
                return match;
            }
        }

        return null;
    }

    private static bool IsDescendant(ScoreCategory category, Guid id)
    {
        foreach (ScoreCategory child in category.SubCategories)
        {
            if (child.Id == id || IsDescendant(child, id))
            {
                return true;
            }
        }

        return false;
    }

    private static void PopulateCategoryEntries(ScoreCategoryManagementModel model)
    {
        model.CategoryEntries.Clear();
        HashSet<Guid> visited = [];
        foreach (ScoreCategory category in model.ScoreCategories.OrderBy(category => category.SortRank))
        {
            AddCategoryEntry(model, category, 0, null, visited);
        }
    }

    private static void AddCategoryEntry(
        ScoreCategoryManagementModel model,
        ScoreCategory category,
        int depth,
        Guid? parentCategoryId,
        ISet<Guid> visited)
    {
        if (!visited.Add(category.Id))
        {
            return;
        }

        model.CategoryEntries.Add(new ScoreCategoryManagementModel.CategoryEntry(category, depth, parentCategoryId));
        foreach (ScoreCategory child in category.SubCategories.OrderBy(child => child.SortRank))
        {
            AddCategoryEntry(model, child, depth + 1, category.Id, visited);
        }
    }

    private static void AddError(ScoreCategoryManagementModel model, string? message, string fallback)
    {
        model.ErrorMessages.Add(string.IsNullOrWhiteSpace(message) ? fallback : message);
    }
}
