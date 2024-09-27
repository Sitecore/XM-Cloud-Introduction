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
public class ScoreCardsViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminScoreCards";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        ScoreCardsModel model = await ModelBinder.Bind<ScoreCardsModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
            result = View(model);
        }
        else
        {
            await Task.WhenAll(
                LoadSelections(model),
                LoadMvpTypes(model));
            if (model.SelectedMvpTypeId > 0 && model.SelectedSelectionId != Guid.Empty)
            {
                await Task.WhenAll(
                    LoadScoreCards(model),
                    LoadTitles(model));
            }

            result = model.ErrorMessages.Count > 0
                ? View("~/Views/Shared/_Error.cshtml", model)
                : View(model);
        }

        return result;
    }

    // ReSharper disable StringLiteralTypo
    private static void GenerateFakeDataForEdit(ScoreCardsModel model)
    {
        Random rnd = new();
        MvpType loremMvpType = new(1) { Name = "Lorem" };
        Country dolorCountry = new(1) { Name = "Dolor" };

        // ReSharper disable once IdentifierTypo - Lorem
        Selection conseceturSelection = new(Guid.NewGuid()) { Year = (short)(DateTime.Now.Year + rnd.Next(10, 20)) };

        model.SelectedSelectionId = conseceturSelection.Id;
        model.SelectedMvpTypeId = 1;
        model.MvpTypes.Add(loremMvpType);
        model.Selections.Add(conseceturSelection);

        model.ScoreCards.Add(new ScoreCard
        {
            Applicant = new Applicant
            {
                MvpType = loremMvpType,
                ApplicationId = Guid.NewGuid(),
                Name = "Ipsum",
                Country = dolorCountry
            },
            Average = rnd.Next(100),
            Median = rnd.Next(100),
            Max = rnd.Next(100),
            Min = rnd.Next(100),
            MaxReviewId = Guid.NewGuid(),
            MinReviewId = Guid.NewGuid(),
            ReviewCount = rnd.Next(10)
        });
        model.ScoreCards.Add(new ScoreCard
        {
            Applicant = new Applicant
            {
                MvpType = loremMvpType,
                ApplicationId = Guid.NewGuid(),
                Name = "Amed",
                Country = dolorCountry
            },
            Average = rnd.Next(100),
            Median = rnd.Next(100),
            Max = rnd.Next(100),
            Min = rnd.Next(100),
            MaxReviewId = Guid.NewGuid(),
            MinReviewId = Guid.NewGuid(),
            ReviewCount = rnd.Next(10)
        });
        model.ScoreCards.Add(new ScoreCard
        {
            Applicant = new Applicant
            {
                MvpType = loremMvpType,
                ApplicationId = Guid.NewGuid(),
                Name = "Sid",
                Country = dolorCountry
            },
            Average = rnd.Next(100),
            Median = rnd.Next(100),
            Max = rnd.Next(100),
            Min = rnd.Next(100),
            MaxReviewId = Guid.NewGuid(),
            MinReviewId = Guid.NewGuid(),
            ReviewCount = rnd.Next(10)
        });
    }

    // ReSharper restore StringLiteralTypo
    private async Task LoadMvpTypes(ScoreCardsModel model)
    {
        Response<IList<MvpType>> mvpTypesResponse = await Client.GetMvpTypesAsync(1, short.MaxValue);
        if (mvpTypesResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.MvpTypes.AddRange(mvpTypesResponse.Result);
        }
        else
        {
            model.ErrorMessages.Add(mvpTypesResponse.Message);
        }
    }

    private async Task LoadSelections(ScoreCardsModel model)
    {
        Response<IList<Selection>> selectionsResponse = await Client.GetSelectionsAsync(1, short.MaxValue);
        if (selectionsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Selections.AddRange(selectionsResponse.Result);
        }
        else
        {
            model.ErrorMessages.Add(selectionsResponse.Message);
        }
    }

    private async Task LoadScoreCards(ScoreCardsModel model)
    {
        Response<IList<ScoreCard>> scoreCardsResponse = await Client.GetScoreCardsAsync(model.SelectedSelectionId, model.SelectedMvpTypeId);
        if (scoreCardsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.ScoreCards.AddRange(scoreCardsResponse.Result);
        }
        else
        {
            model.ErrorMessages.Add(scoreCardsResponse.Message);
        }
    }

    private async Task LoadTitles(ScoreCardsModel model)
    {
        short? year = model.Selections.SingleOrDefault(s => s.Id == model.SelectedSelectionId)?.Year;
        if (year.HasValue)
        {
            Response<IList<Title>> titlesResponse = await Client.GetTitlesAsync(null, null, [year.Value], null, 1, short.MaxValue);
            if (titlesResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.Titles.AddRange(titlesResponse.Result);
            }
            else
            {
                model.ErrorMessages.Add(titlesResponse.Message);
            }
        }
    }
}