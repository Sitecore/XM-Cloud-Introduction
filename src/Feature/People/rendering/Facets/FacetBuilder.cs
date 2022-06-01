using Mvp.Feature.People.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mvp.Feature.People.Facets
{
    public class FacetBuilder : IFacetBuilder
    {
        public IDictionary<string, List<Facet>> CalculateFacets(IList<MvpSearchResult> mvps, SearchParams searchParams)
        {
            var facetDictionary = new Dictionary<string, List<Facet>>
            {
                { "Type", new List<Facet>() },
                { "Year", new List<Facet>() },
                { "Country", new List<Facet>() }
            };

            foreach (var mvp in mvps)
            {
                ProcessCountryFacets(facetDictionary, mvp, searchParams);
                var addedAwardTypes = new List<string>();
                foreach (var award in mvp.Awards.TargetItems)
                {
                    ProcessYearFacets(facetDictionary, award, searchParams);
                    ProcessAwardTypeFacets(facetDictionary, award, addedAwardTypes, searchParams);
                }
            }

            SortFacetLists(facetDictionary);
            return facetDictionary;
        }

        private static void SortFacetLists(Dictionary<string, List<Facet>> facetDictionary)
        {
            facetDictionary["Type"] = facetDictionary["Type"].OrderByDescending(x => x.Count).ToList();
            facetDictionary["Year"] = facetDictionary["Year"].OrderByDescending(x => x.Name).ToList();
            facetDictionary["Country"] = facetDictionary["Country"].OrderBy(x => x.Name).ToList();
        }

        private static void ProcessAwardTypeFacets(Dictionary<string, List<Facet>> facetDictionary, Awards award, List<string> addedAwardTypes, SearchParams searchParams)
        {
            var awardName = award.Field.TargetItem.Field.Value;
            if (!addedAwardTypes.Exists(x => x == awardName))
            {
                if (!facetDictionary["Type"].Exists(x => x.Name == awardName))
                {
                    var facet = new Facet
                    {
                        Name = awardName,
                        Count = 1,
                        Selected = !string.IsNullOrWhiteSpace(searchParams.Award) && searchParams.Award.ToLowerInvariant().Contains(awardName.ToLowerInvariant())
                    };
                    facetDictionary["Type"].Add(facet);
                }
                else
                    facetDictionary["Type"].First(x => x.Name == awardName).Count++;
            }

            addedAwardTypes.Add(awardName);
        }

        private static void ProcessYearFacets(Dictionary<string, List<Facet>> facetDictionary, Awards award, SearchParams searchParams)
        {
            var yearName = award.Parent.Name;
            if (!facetDictionary["Year"].Exists(x => x.Name == yearName))
            {
                var facet = new Facet
                {
                    Name = yearName,
                    Count = 1,
                    Selected = !string.IsNullOrWhiteSpace(searchParams.Year) && searchParams.Year.ToLowerInvariant().Contains(yearName.ToLowerInvariant())
                };
                facetDictionary["Year"].Add(facet);
            }
            else
                facetDictionary["Year"].First(x => x.Name == yearName).Count++;
        }

        private static void ProcessCountryFacets(Dictionary<string, List<Facet>> facetDictionary, MvpSearchResult mvp, SearchParams searchParams)
        {
            var countryName = mvp.Country?.TargetItem?.Name;
            if (!string.IsNullOrWhiteSpace(countryName))
            {
                if (!facetDictionary["Country"].Exists(x => x.Name == countryName))
                {
                    var facet = new Facet
                    {
                        Name = countryName,
                        Count = 1,
                        Selected = !string.IsNullOrWhiteSpace(searchParams.Country) && searchParams.Country.ToLowerInvariant().Contains(countryName.ToLowerInvariant())
                    };
                    facetDictionary["Country"].Add(facet);
                }
                else
                    facetDictionary["Country"].First(x => x.Name == countryName).Count++;
            }
        }
    }
}
