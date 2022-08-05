using Mvp.Foundation.DataFetching.GraphQL.Models;

namespace Mvp.Feature.Forms.Models
{
    public class ApplicationListData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ValueItem Description { get; set; }
        public Template Template { get; set; }
        public BoolField Active { get; set; }
    }

    public class ApplicationListDataSearchResponse
    {
        public ApplicationListDataSearch Search { get; set; }
    }

    public class ApplicationListDataSearch
    {
        public ApplicationListData[] Results { get; set; }
        public int Total { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
