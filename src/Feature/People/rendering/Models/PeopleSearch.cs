using Mvp.Foundation.DataFetching.GraphQL.Models;

namespace Mvp.Feature.People.Models;

public class PeopleSearch
{
    public MvpSearchResult[] Results { get; set; }
    public int Total { get; set; }
    public PageInfo PageInfo { get; set; }
}