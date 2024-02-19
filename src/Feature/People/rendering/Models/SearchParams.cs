using Microsoft.AspNetCore.Mvc;

namespace Mvp.Feature.People.Models;

public class SearchParams
{
    [FromQuery(Name = Constants.QueryParameters.Page)]
    public int CurrentPage { get; set; }

    [FromQuery(Name = Constants.QueryParameters.Query)]
    public string Keyword { get; set; }

    [FromQuery(Name = Constants.QueryParameters.FacetAward)]
    public string Award { get; set; }

    [FromQuery(Name = Constants.QueryParameters.FacetYear)]
    public string Year { get; set; }

    [FromQuery(Name = Constants.QueryParameters.FacetCountry)]
    public string Country { get; set; }
}