using Microsoft.AspNetCore.Mvc;
using MvpSite.Rendering.Models.Filters;

namespace MvpSite.Rendering.Models;

public abstract class ListModel<T> : BaseModel
{
    [FromQuery(Name = "p")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "ps")]
    public short PageSize { get; set; } = 50;

    public List<T> List { get; } = [];

    public BaseFilter Filter { get; set; } = new BaseFilter.None();
}