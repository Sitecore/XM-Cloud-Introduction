using System.Collections.Generic;
using System.Linq;
using Mvp.Feature.People.ViewComponents;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.People.Models.Directory
{
    public class FacetViewModel
    {
        public string Identifier { get; set; }
        
        public TextField Name { get; set; }

        public List<FacetOption> FacetOptions { get; set; } = [];

        public int SortOrder { get; set; }

        public string ToQueryString()
        {
            string result = FacetOptions
                .Where(fo => fo.IsActive)
                .Aggregate(string.Empty, (current, option) => current + $"{DirectoryViewComponent.FacetQuerystringPrefix}{Identifier}={option.Identifier}&");

            return !string.IsNullOrWhiteSpace(result) ? result[..^1] : string.Empty;
        }
    }
}
