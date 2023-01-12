using System;
using System.Collections.Generic;
using System.Linq;
using Mvp.Selections.Domain;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class ScoreCardDetailModel : BaseModel
    {
        public Guid ApplicationId { get; set; } = Guid.Empty;

        public Application Application { get; set; }

        public List<Review> Reviews { get; set; } = new ();

        public List<ScoreCategory> ScoreCategories { get; set; } = new ();

        public string ErrorMessage { get; set; } = string.Empty;

        public TextField TitleLabel { get; set; }

        public TextField ReviewsLabel { get; set; }

        public TextField ApplicationLabel { get; set; }

        public static decimal CalculateValue(ScoreCategory category, Review review)
        {
            decimal categoryScoreValue = 0;
            ReviewCategoryScore current =
                review.CategoryScores.SingleOrDefault(rcs => rcs.ScoreCategoryId == category.Id);
            if (current != null)
            {
                categoryScoreValue += category.ScoreOptions.Single(so => so.Id == current.ScoreId).Value;
            }

            categoryScoreValue += category.SubCategories.Sum(subCategory => CalculateValue(subCategory, review));
            return categoryScoreValue * category.Weight;
        }

        public static string ScoreDisplayName(ScoreCategory category, Review review)
        {
            string result = string.Empty;
            ReviewCategoryScore current =
                review.CategoryScores.SingleOrDefault(rcs => rcs.ScoreCategoryId == category.Id);
            if (current != null)
            {
                result = category.ScoreOptions.Single(so => so.Id == current.ScoreId).Name;
            }

            return result;
        }
    }
}
