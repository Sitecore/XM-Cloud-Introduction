using Mvp.Foundation.DataFetching.GraphQL.Models;

namespace Mvp.Feature.People.Models;

public class MvpSearchResult
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public ValueItem FirstName { get; set; }
    public ValueItem LastName { get; set; }
    public ValueItem Email { get; set; }
    public CountryData Country { get; set; }
    public AwardData Awards { get; set; }
}