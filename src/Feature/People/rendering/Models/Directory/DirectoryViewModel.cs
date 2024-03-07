using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.People.Models.Directory
{
    public class DirectoryViewModel
    {
        public const string QueryQueryStringKey = "q";

        public const string PageQueryStringKey = "pg";
        
        [SitecoreContextProperty]
        public bool IsEditing { get; set; }
        
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
    }
}
