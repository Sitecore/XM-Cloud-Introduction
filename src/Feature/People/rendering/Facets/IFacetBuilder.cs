using Mvp.Feature.People.Models;
using System.Collections.Generic;

namespace Mvp.Feature.People.Facets
{
    public interface IFacetBuilder
    {
        public IDictionary<string, List<Facet>> CalculateFacets(IList<MvpSearchResult> mvps, SearchParams searchParams);
    }
}
