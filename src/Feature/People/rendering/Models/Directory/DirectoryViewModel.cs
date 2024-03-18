using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.People.Models.Directory
{
    public class DirectoryViewModel : BaseModel
    {
        public const string QueryQueryStringKey = "q";

        public const string PageQueryStringKey = "pg";
        
        public TextField TopTitleLabel { get; set; }

        public TextField TitleLabel { get; set; }

        public TextField SearchLabel { get; set; }

        public ContentListField Facets { get; set; } = [];

        public List<FacetViewModel> ViewFacets { get; set; } = [];

        public TextField PagingResults { get; set; }

        public TextField PagingResultsFormat { get; set; } = new("{0} - {1} of {2}");

        public int FirstResult => (Page - 1) * PageSize + 1;

        public int LastResult => Page * PageSize > TotalResults ? TotalResults : Page * PageSize;

        public int TotalResults { get; set; }

        public List<DirectoryResultViewModel> Results { get; set; } = [];

        public HyperLinkField MvpProfileLink { get; set; }

        [FromQuery(Name = QueryQueryStringKey)]
        public string Query { get; set; }

        [FromQuery(Name = PageQueryStringKey)]
        public int Page { get; set; } = 1;

        public short PageSize { get; set; } = 21;

        public int StartPage => Page >= 3 ? Page - 2 : Page >= 2 ? Page - 1 : Page;

        public int EndPage => StartPage + 4 > TotalPages ? TotalPages : StartPage + 4;

        public int TotalPages => TotalResults / PageSize + 1;

        public Uri PageUri(int page)
        {
            return new Uri(
                $"?{PageQueryStringKey}={page}&{string.Join('&', ViewFacets.Select(vf => vf.ToQueryString()).Where(s => !string.IsNullOrWhiteSpace(s)))}",
                UriKind.Relative);
        }

        public Uri NextPageUri()
        {
            int nextPage = Page + 1;
            return nextPage <= TotalPages ? PageUri(nextPage) : new Uri("#", UriKind.Relative);
        }

        public Uri PreviousPageUri()
        {
            int previousPage = Page - 1;
            return previousPage > 0 ? PageUri(previousPage) : new Uri("#", UriKind.Relative);
        }
    }
}
