using Mvp.Foundation.DataFetching.GraphQL.Models;

namespace Mvp.Feature.People.Models;

public class Awards
{
    public NameItem Parent { get; set; }

    public TargetItemFieldValueItem Field { get; set; }
}