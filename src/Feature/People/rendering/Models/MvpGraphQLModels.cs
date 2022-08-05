using Microsoft.AspNetCore.Mvc;
using Mvp.Foundation.DataFetching.GraphQL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Feature.People.Models
{
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

    public class PeopleSearchResults
    {
        public IEnumerable<Person> People { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IDictionary<string, List<Facet>> Facets { get; set; }
        public bool HasNextPage
        {
            get
            {
                return CurrentPage < LastPage;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public int LastPage
        {
            get
            {
                if (TotalCount == 0 || PageSize == 0)
                {
                    return 1;
                }
                return (int)Math.Ceiling(((decimal)TotalCount) / ((decimal)PageSize));
            }
        }

        public int PagerStart
        {
            get
            {
                if (CurrentPage > 3)
                {
                    return CurrentPage - 2;
                }
                else
                {
                    return 1;
                }
            }
        }

        public int PagerEnd
        {
            get
            {
                if (!HasNextPage)
                {
                    return CurrentPage;
                }

                if (CurrentPage + 2 >= LastPage)
                {
                    return LastPage;
                }
                if (CurrentPage < 2 && LastPage > 5)
                {
                    return CurrentPage + 4;
                }
                if (CurrentPage < 3 && LastPage > 5)
                {
                    return CurrentPage + 3;
                }
                if (CurrentPage < 3 && LastPage <= 5)
                {
                    return LastPage;
                }
                return CurrentPage + 2;
            }
        }
    }

    public class PeopleSearch
    {
        public MvpSearchResult[] Results { get; set; }
        public int Total { get; set; }
        public PageInfo PageInfo { get; set; }
    }

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

    public class AwardData
    {
        public Awards[] TargetItems { get; set; }
    }

    public class CountryData
    {
        public TargetItem TargetItem { get; set; }
    }

    public class Awards
    {
        public NameItem Parent { get; set; }

        public TargetItemFieldValueItem Field { get; set; }
    }

    public class MvpSearchResponse
    {
        public PeopleSearch Search { get; set; }
    }

    public class Facet
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Selected { get; set; }
    }

    public class Country
    {
        public TargetItem targetItem { get; set; }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Country { get; set; }
        public string MvpYear { get; set; }
        public string MvpCategory { get; set; }
    }
}