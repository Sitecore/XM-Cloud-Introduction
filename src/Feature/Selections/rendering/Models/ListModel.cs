using Microsoft.AspNetCore.Mvc;

namespace Mvp.Feature.Selections.Models
{
    public abstract class ListModel : BaseModel
    {
        [FromQuery(Name = "p")]
        public int Page { get; set; }

        [FromQuery(Name = "ps")]
        public short PageSize { get; set; }
    }
}
