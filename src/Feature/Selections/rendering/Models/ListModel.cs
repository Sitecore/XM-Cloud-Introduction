using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Filters;

namespace Mvp.Feature.Selections.Models
{
    public abstract class ListModel<T> : BaseModel
    {
        [FromQuery(Name = "p")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "ps")]
        public short PageSize { get; set; } = 50;

        public List<T> List { get; } = new ();

        public BaseFilter Filter { get; set; } = new BaseFilter.None();
    }
}
