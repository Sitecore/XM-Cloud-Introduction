using Microsoft.AspNetCore.Mvc;
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

        //public string Language { get; set; }
        //public string RootItemId { get; set; }
        //public int? PageSize { get; set; }

        //public int? CursorValueToGetItemsAfter { get; set; }

        //public bool? IsInEditingMode { get; set; }

        //public IList<(KeyValuePair<string, string>, IDictionary<string, string>)>? FilterFacets { get; set; }
        //public List<KeyValuePair<string, string[]>>? Facets { get; set; }

        //public IList<string> FacetOn { get; set; }
        //public string Query { get; set; }

        //public string CacheKey { get; set; }
    }

    public class PeopleSearchResults
    {
        public IEnumerable<Person> People { get; set; }
        //public List<Facet> Facets { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }       
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

        //public IList<(KeyValuePair<string, string>, IDictionary<string, string>)>? FilterFacets { get; set; }

        //public string keyword { get; set; }

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
        //public List<Facet> facets { get; set; }

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
        public TargetItem Country { get; set; }
        public AwardData Awards { get; set; }
    }

    public class AwardData
    {
        public Awards[] TargetItems { get; set; }
    }

    public class Awards
    {
        public NameItem Parent { get; set; }

        public TargetItemFieldValueItem Field { get; set; }

    }

    public class NameItem
    {
        public string Name { get; set; }
    }

    public class MvpSearchResponse
    {
        public PeopleSearch Search { get; set; }
    }


    //public class Value
    //{
    //    public string value { get; set; }
    //    public bool isChecked { get; set; }
    //    public int count { get; set; }
    //}

    //public class Facet
    //{
    //    public string name { get; set; }
    //    public string DisplayName { get; set; }
    //    public List<Value> values { get; set; }

    //}

    public class ValueItem
    {
        public string Value { get; set; }
    }


    //	public class Introduction
    //	{
    //		public string value { get; set; }
    //	}

    public class TargetItemFieldValueItem
    {
        public FieldValueItem TargetItem { get; set; }
    }

    public class FieldValueItem
    {
        public ValueItem Field { get; set; }
    }

    public class TargetItem
    {
        public string Name { get; set; }
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
       // public Introduction introduction { get; set; }
        public string Url { get; set; }
        public string Country { get; set; }
        public string MvpYear { get; set; }
        public string MvpCategory { get; set; }
    }


    //	public class SearchItem
    //	{
    //		public Person item { get; set; }
    //	}

    public class PageInfo
    {
        //public string startCursor { get; set; }
        public string endCursor { get; set; }
        public bool hasNextPage { get; set; }
        //public bool hasPreviousPage { get; set; }
    }

    //public class Results
    //{
    //    public List<SearchItem> items { get; set; }
    //    public int totalCount { get; set; }
    //    public PageInfo pageInfo { get; set; }
    //}

    //	public class FieldFilter
    //	{
    //		public string name { get; set; }
    //		public string value { get; set; }
    //	}

}
