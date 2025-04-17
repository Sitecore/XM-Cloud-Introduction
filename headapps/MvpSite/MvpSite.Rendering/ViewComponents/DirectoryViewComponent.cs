using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using MvpSite.Rendering.Configuration;
using MvpSite.Rendering.Extensions;
using MvpSite.Rendering.Helpers;
using MvpSite.Rendering.Models.Directory;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents;

[ViewComponent(Name = ViewComponentName)]
public class DirectoryViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client, IMemoryCache cache, IOptions<MvpPeopleOptions> options)
    : ViewComponent
{
    public const string ViewComponentName = "Directory";

    public const string FacetQuerystringPrefix = "fc_";

    public const string MvpTypeFacetIdentifier = "type";

    public const string YearFacetIdentifier = "year";

    public const string CountryFacetIdentifier = "country";

    public const string MentorFacetIdentifier = "mentor";

    public const string MentorFacetOptionIdentifier = "mentor";

    public const string OpenToMenteesOptionIdentifier = "openToMentees";

    private const string SearchCacheKeyPrefix = "SearchMvpProfileAsync_";

    private readonly MvpPeopleOptions _options = options.Value;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        DirectoryViewModel model = await modelBinder.Bind<DirectoryViewModel>(ViewContext);
        ParseQuerystringFacets(model);
        await ExecuteSearch(model);
        MergeFacetsAndViewFacets(model);

        return model.ErrorMessages.Count > 0 ? View("~/Views/Shared/Components/Directory/Error.cshtml", model) :
            model.TotalResults == 0 ? View("~/Views/Shared/Components/Directory/NoResults.cshtml", model) :
            View(model);
    }

    private static void MergeFacetsAndViewFacets(DirectoryViewModel model)
    {
        List<string> removeIdentifiers = [];
        foreach (FacetViewModel facet in model.ViewFacets)
        {
            ItemLinkField? cmsFacet = model.Facets.FirstOrDefault(f =>
                f.Fields.TryGetValue(nameof(FacetViewModel.Identifier), out IFieldReader? identifierReader)
                && identifierReader.TryRead(out TextField? identifierField)
                && identifierField?.Value == facet.Identifier);

            if (cmsFacet != null
                && cmsFacet.Fields.TryGetValue(nameof(FacetViewModel.Name), out IFieldReader? nameReader)
                && nameReader.TryRead(out TextField? nameField))
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

    private static List<short>? ExtractSearchIdentifiers(IEnumerable<FacetViewModel> facets, string identifier)
    {
        List<short>? result = null;
        FacetViewModel? facet = facets.FirstOrDefault(f => f.Identifier == identifier);
        if (facet != null)
        {
            List<short?> identifiers = facet.FacetOptions
                .Select(o => short.TryParse(o.Identifier, out short v) ? (short?)v : null).Where(s => s != null).ToList();
            if (identifiers.Count > 0)
            {
                result = identifiers.Cast<short>().ToList();
            }
        }

        return result;
    }

    // NOTE [IVA] We only want null or true for facets otherwise we accidentally use negative filtering which creates unexpected results
    private static bool? ExtractFacetBoolean(IEnumerable<FacetViewModel> facets, string facetIdentifier, string optionIdentifier)
    {
        bool? result = null;
        FacetViewModel? facet = facets.FirstOrDefault(f => f.Identifier == facetIdentifier);
        if (
            facet != null &&
            (facet.FacetOptions.FirstOrDefault(o => o.Identifier == optionIdentifier)?.IsActive ?? false))
        {
            result = true;
        }

        return result;
    }

    private static void MergeFacetOptions(FacetViewModel viewFacet, IEnumerable<SearchFacetOption> options)
    {
        foreach (SearchFacetOption option in options)
        {
            FacetOption? viewOption =
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

        viewFacet.FacetOptions = viewFacet.Identifier switch
        {
            YearFacetIdentifier => [..viewFacet.FacetOptions.OrderByDescending(o => o.Display)],
            _ => [..viewFacet.FacetOptions.OrderBy(o => o.Display)]
        };
    }

    private async Task ExecuteSearch(DirectoryViewModel model)
    {
        List<short>? mvpTypeFacetIdentifiers = ExtractSearchIdentifiers(model.ViewFacets, MvpTypeFacetIdentifier);
        List<short>? yearFacetIdentifiers = ExtractSearchIdentifiers(model.ViewFacets, YearFacetIdentifier);
        List<short>? countryFacetIdentifiers = ExtractSearchIdentifiers(model.ViewFacets, CountryFacetIdentifier);
        bool? mentor = ExtractFacetBoolean(model.ViewFacets, MentorFacetIdentifier, MentorFacetOptionIdentifier);
        bool? openToMentees = ExtractFacetBoolean(model.ViewFacets, MentorFacetIdentifier, OpenToMenteesOptionIdentifier);
        string cacheKey =
            $"{SearchCacheKeyPrefix}{model.Query}_{mvpTypeFacetIdentifiers.ToCommaSeparatedStringOrNullLiteral()}_{yearFacetIdentifiers.ToCommaSeparatedStringOrNullLiteral()}_{countryFacetIdentifiers.ToCommaSeparatedStringOrNullLiteral()}_{mentor.ToStringOrNullLiteral()}_{openToMentees.ToStringOrNullLiteral()}_p{model.Page}/{model.PageSize}";
        bool isAdmin = await new MvpSelectionsApiHelper(client).IsCurrentUserAnAdmin();
        if (isAdmin || !cache.TryGetValue(cacheKey, out SearchResult<MvpProfile>? profiles))
        {
            Response<SearchResult<MvpProfile>> response = await client.SearchMvpProfileAsync(
                model.Query,
                mvpTypeFacetIdentifiers,
                yearFacetIdentifiers,
                countryFacetIdentifiers,
                mentor,
                openToMentees,
                model.Page,
                model.PageSize);
            if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                profiles = response.Result;
                if (!isAdmin)
                {
                    cache.Set(cacheKey, response.Result, TimeSpan.FromSeconds(_options.SearchCachedSeconds));
                }
            }
            else
            {
                profiles = null;
                model.ErrorMessages.Add(response.Message);
            }
        }

        if (profiles != null)
        {
            model.TotalResults = profiles.TotalResults;
            model.Page = profiles.Page;
            model.PageSize = profiles.PageSize;
            foreach (MvpProfile profile in profiles.Results)
            {
                model.Results.Add(DirectoryResultViewModel.FromMvpProfile(profile, model.MvpProfileLink?.ToUri()));
            }

            foreach (SearchFacet facet in profiles.Facets)
            {
                FacetViewModel? viewFacet = model.ViewFacets.FirstOrDefault(f => f.Identifier == facet.Identifier);
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
            FacetViewModel? facet = model.ViewFacets.FirstOrDefault(f =>
                qs.Key.Equals(FacetQuerystringPrefix + f.Identifier, StringComparison.InvariantCultureIgnoreCase));
            if (facet != null)
            {
                foreach (string? value in qs.Value)
                {
                    FacetOption? facetOption = facet.FacetOptions.FirstOrDefault(o =>
                        o.Identifier.Equals(value, StringComparison.InvariantCultureIgnoreCase));
                    if (facetOption != null)
                    {
                        facetOption.IsActive = true;
                    }
                    else if (value != null)
                    {
                        facet.FacetOptions.Add(new FacetOption { IsActive = true, Identifier = value });
                    }
                }
            }
            else
            {
                facet = new FacetViewModel { Identifier = qs.Key[FacetQuerystringPrefix.Length..] };
                foreach (string? value in qs.Value.OfType<string>())
                {
                    facet.FacetOptions.Add(new FacetOption { IsActive = true, Identifier = value });
                }

                model.ViewFacets.Add(facet);
            }
        }
    }
}