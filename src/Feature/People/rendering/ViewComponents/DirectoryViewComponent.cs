using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Sitecore.AspNet.RenderingEngine.Binding;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Mvp.Feature.People.Extensions;
using Mvp.Feature.People.Models.Directory;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Client.Models;
using Sitecore.LayoutService.Client.Response.Model;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.People.ViewComponents
{
    [ViewComponent(Name = ViewComponentName)]
    public class DirectoryViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
        : ViewComponent
    {
        public const string ViewComponentName = "Directory";

        public const string FacetQuerystringPrefix = "fc_";

        public const string MvpTypeFacetIdentifier = "type";
        
        public const string YearFacetIdentifier = "year";

        public const string CountryFacetIdentifier = "country";

        public async Task<IViewComponentResult> InvokeAsync()
        {
            DirectoryViewModel model = await modelBinder.Bind<DirectoryViewModel>(ViewContext);
            ParseQuerystringFacets(model);
            await ExecuteSearch(model);
            MergeFacetsAndViewFacets(model);

            return View(model);
        }

        private static void MergeFacetsAndViewFacets(DirectoryViewModel model)
        {
            List<string> removeIdentifiers = [];
            foreach (FacetViewModel facet in model.ViewFacets)
            {
                ItemLinkField cmsFacet = model.Facets.FirstOrDefault(f =>
                    f.Fields.TryGetValue(nameof(FacetViewModel.Identifier), out IFieldReader identifierReader)
                    && identifierReader.TryRead(out TextField identifierField)
                    && identifierField.Value == facet.Identifier);

                if (cmsFacet != null
                    && cmsFacet.Fields.TryGetValue(nameof(FacetViewModel.Name), out IFieldReader nameReader)
                    && nameReader.TryRead(out TextField nameField))
                {
                    facet.Name = nameField;
                    facet.SortOrder = model.Facets.IndexOf(cmsFacet);
                }
                else
                {
                    removeIdentifiers.Add(facet.Identifier);
                }
            }

            model.ViewFacets.RemoveAll(f => removeIdentifiers.Contains(f.Identifier));
        }

        private static IEnumerable<short> ExtractSearchIdentifiers(IEnumerable<FacetViewModel> facets, string identifier)
        {
            IEnumerable<short> result = null;
            FacetViewModel facet = facets.FirstOrDefault(f => f.Identifier == identifier);
            if (facet != null)
            {
                List<short?> identifiers = facet.FacetOptions
                    .Select(o => short.TryParse(o.Identifier, out short v) ? (short?)v : null).Where(s => s != null).ToList();
                if (identifiers.Count > 0)
                {
                    result = identifiers.Cast<short>();
                }
            }

            return result;
        }

        private static void MergeFacetOptions(FacetViewModel viewFacet, IEnumerable<SearchFacetOption> options)
        {
            foreach (SearchFacetOption option in options)
            {
                FacetOption viewOption =
                    viewFacet.FacetOptions.FirstOrDefault(o => o.Identifier == option.Identifier);
                if (viewOption != null)
                {
                    viewOption.Display = option.Display;
                    viewOption.Count = option.Count;
                }
                else
                {
                    viewFacet.FacetOptions.Add(new FacetOption { Identifier = option.Identifier, Display = option.Display, Count = option.Count });
                }
            }

            switch (viewFacet.Identifier)
            {
                case YearFacetIdentifier:
                    viewFacet.FacetOptions = [..viewFacet.FacetOptions.OrderByDescending(o => o.Display)];
                    break;
                default:
                    viewFacet.FacetOptions = [..viewFacet.FacetOptions.OrderBy(o => o.Display)];
                    break;
            }
        }

        private async Task ExecuteSearch(DirectoryViewModel model)
        {
            Response<SearchResult<MvpProfile>> response = await client.SearchMvpProfileAsync(
                model.Query,
                ExtractSearchIdentifiers(model.ViewFacets, MvpTypeFacetIdentifier),
                ExtractSearchIdentifiers(model.ViewFacets, YearFacetIdentifier),
                ExtractSearchIdentifiers(model.ViewFacets, CountryFacetIdentifier),
                model.Page,
                model.PageSize);
            if (response.StatusCode == HttpStatusCode.OK && response.Result != null)
            {
                model.TotalResults = response.Result.TotalResults;
                model.Page = response.Result.Page;
                model.PageSize = response.Result.PageSize;
                foreach (MvpProfile profile in response.Result.Results)
                {
                    model.Results.Add(DirectoryResultViewModel.FromMvpProfile(profile, model.MvpProfileLink?.ToUri()));
                }

                foreach (SearchFacet facet in response.Result.Facets)
                {
                    FacetViewModel viewFacet = model.ViewFacets.FirstOrDefault(f => f.Identifier == facet.Identifier);
                    if (viewFacet is null)
                    {
                        viewFacet = new FacetViewModel { Identifier = facet.Identifier };
                        model.ViewFacets.Add(viewFacet);
                    }

                    MergeFacetOptions(viewFacet, facet.Options);
                }
            }
        }

        private void ParseQuerystringFacets(DirectoryViewModel model)
        {
            foreach (KeyValuePair<string, StringValues> qs in HttpContext.Request.Query.Where(q => q.Key.StartsWith(FacetQuerystringPrefix)))
            {
                FacetViewModel facet = model.ViewFacets.FirstOrDefault(f =>
                    qs.Key.Equals(FacetQuerystringPrefix + f.Identifier, StringComparison.InvariantCultureIgnoreCase));
                if (facet != null)
                {
                    foreach (string value in qs.Value)
                    {
                        FacetOption facetOption = facet.FacetOptions.FirstOrDefault(o =>
                            o.Identifier.Equals(value, StringComparison.InvariantCultureIgnoreCase));
                        if (facetOption != null)
                        {
                            facetOption.IsActive = true;
                        }
                        else
                        {
                            facet.FacetOptions.Add(new FacetOption { IsActive = true, Identifier = value });
                        }
                    }
                }
                else
                {
                    facet = new FacetViewModel { Identifier = qs.Key[FacetQuerystringPrefix.Length..] };
                    foreach (string value in qs.Value)
                    {
                        facet.FacetOptions.Add(new FacetOption { IsActive = true, Identifier = value });
                    }
                    
                    model.ViewFacets.Add(facet);
                }
            }
        }
    }
}
