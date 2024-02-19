using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Sitecore.AspNet.RenderingEngine.Binding;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Mvp.Feature.People.Models.Directory;
using Sitecore.LayoutService.Client.Response.Model.Fields;
using Sitecore.LayoutService.Client.Response.Model.Properties;

namespace Mvp.Feature.People.ViewComponents
{
    [ViewComponent(Name = ViewComponentName)]
    public class DirectoryViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
        : ViewComponent
    {
        public const string ViewComponentName = "Directory";

        public const string FacetQuerystringPrefix = "fc_";

        public async Task<IViewComponentResult> InvokeAsync()
        {
            DirectoryViewModel model = await modelBinder.Bind<DirectoryViewModel>(ViewContext);
            model.Facets.Add(new FacetViewModel
            {
                Identifier = "type",
                Name = new TextField("Type"),
                FacetOptions =
                [
                    new FacetOption { Identifier = "1234", Display = "Technology", Count = 2 },
                    new FacetOption { Identifier = "5412", Display = "Strategy", Count = 1 }
                ]
            });
            model.Facets.Add(new FacetViewModel
            {
                Identifier = "year",
                Name = new TextField("Year"),
                FacetOptions =
                [
                    new FacetOption { Identifier = "2020", Display = "2020", Count = 2 },
                    new FacetOption { Identifier = "2021", Display = "2021", Count = 1 }
                ]
            });
            model.FirstResult = 1 + ((model.Page - 1) * model.PageSize);
            model.LastResult = model.PageSize * model.Page;
            model.TotalResults = 861;
            model.MvpProfileLink = new HyperLinkField(new HyperLink { Href = "/profile/test", Text = "Mvp Profile" });
            for (int i = model.FirstResult; i <= (model.LastResult > model.TotalResults ? model.TotalResults : model.LastResult); i++)
            {
                model.Results.Add(new DirectoryResultViewModel
                {
                    Name = "Test Tester " + i,
                    Country = "Testland",
                    Year = "2024",
                    Type = "Technology",
                    Image = new Uri("/images/mvp-base-user-grey.png", UriKind.Relative),
                    ProfileUri = new Uri(model.MvpProfileLink.Value.Href, UriKind.RelativeOrAbsolute)
                });
            }

            ParseQuerystringFacets(model);

            return View(model);
        }

        private void ParseQuerystringFacets(DirectoryViewModel model)
        {
            foreach (KeyValuePair<string, StringValues> qs in HttpContext.Request.Query)
            {
                FacetViewModel facet = model.Facets.FirstOrDefault(f =>
                    qs.Key.Equals(FacetQuerystringPrefix + f.Identifier, StringComparison.InvariantCultureIgnoreCase));
                if (facet != null)
                {
                    foreach (FacetOption option in qs.Value.Select(value => facet.FacetOptions.FirstOrDefault(o => o.Identifier.Equals(value))).Where(option => option != null))
                    {
                        option.IsActive = true;
                    }
                }
            }
        }
    }
}
