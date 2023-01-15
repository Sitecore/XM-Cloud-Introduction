using System;
using System.Collections.Generic;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class ScoreCardsModel : BaseModel
    {
        public List<Selection> Selections { get; set; } = new ();

        public Guid SelectedSelectionId { get; set; } = Guid.Empty;

        public List<MvpType> MvpTypes { get; set; } = new ();

        public short SelectedMvpTypeId { get; set; } = 0;

        public List<ScoreCard> ScoreCards { get; set; } = new ();

        public List<Title> Titles { get; set; } = new ();

        public string ErrorMessage { get; set; } = string.Empty;

        public TextField TitleLabel { get; set; }

        public TextField SelectionLabel { get; set; }

        public TextField MvpTypeLabel { get; set; }

        public TextField SubmitLabel { get; set; }

        public TextField AwardedLabel { get; set; }

        public TextField NameTableHeader { get; set; }

        public TextField CountryTableHeader { get; set; }

        public TextField ReviewCountTableHeader { get; set; }

        public TextField AverageTableHeader { get; set; }

        public TextField MedianTableHeader { get; set; }

        public TextField MinTableHeader { get; set; }

        public TextField MaxTableHeader { get; set; }

        public HyperLinkField DetailLink { get; set; }

        public HyperLinkField AwardLink { get; set; }

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
}
