using System.Collections.Generic;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.People.Models.Directory
{
    public class FacetViewModel
    {
        public string Identifier { get; set; }
        
        public TextField Name { get; set; }

        public List<FacetOption> FacetOptions { get; set; } = [];

        public int SortOrder { get; set; }
    }
}
