﻿using Mvp.Selections.Api.Model;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin;

public class ScoreCardsModel : BaseModel
{
    public List<Selection> Selections { get; set; } = [];

    public Guid SelectedSelectionId { get; set; } = Guid.Empty;

    public List<MvpType> MvpTypes { get; set; } = [];

    public short SelectedMvpTypeId { get; set; }

    public List<ScoreCard> ScoreCards { get; set; } = [];

    public List<Title> Titles { get; set; } = [];

    public TextField? TitleLabel { get; set; }

    public TextField? SelectionLabel { get; set; }

    public TextField? MvpTypeLabel { get; set; }

    public TextField? SubmitLabel { get; set; }

    public TextField? AwardedLabel { get; set; }

    public TextField? NameTableHeader { get; set; }

    public TextField? CountryTableHeader { get; set; }

    public TextField? ReviewCountTableHeader { get; set; }

    public TextField? AverageTableHeader { get; set; }

    public TextField? MedianTableHeader { get; set; }

    public TextField? MinTableHeader { get; set; }

    public TextField? MaxTableHeader { get; set; }

    public HyperLinkField? DetailLink { get; set; }

    public HyperLinkField? AwardLink { get; set; }

    public static string Color(int score)
    {
        return score switch
        {
            > 50 => "green",
            > 30 => "#FC0",
            > 0 => "#8B0001",
            _ => "red"
        };
    }
}