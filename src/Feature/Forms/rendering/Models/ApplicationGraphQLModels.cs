using Mvp.Foundation.DataFetching.GraphQL.Models;

namespace Mvp.Feature.Forms.Models
{
    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ValueItem Description { get; set; }
    }

    public class CountrySearchResponse
    {
        public CountrySearch Search { get; set; }
    }

    public class CountrySearch
    {
        public Country[] Results { get; set; }
        public int Total { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
