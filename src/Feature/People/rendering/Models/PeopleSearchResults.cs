using System;
using System.Collections.Generic;

namespace Mvp.Feature.People.Models;

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