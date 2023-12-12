using DotnetSitemapGenerator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mvp.Foundation.DataFetching.GraphQL;
using Mvp.Project.MvpSite.Middleware;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Rendering;

public class Mvp
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
}

public class MvpSitemapUrlProvider : ISitemapUrlProvider
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<MvpSitemapUrlProvider> _logger;
    private readonly IGraphQLClientFactory _graphQLClientFactory;
    private readonly IGraphQLRequestBuilder _graphQLRequestBuilder;
    private readonly IConfiguration _configuration;

    public MvpSitemapUrlProvider(
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor,
        ILogger<MvpSitemapUrlProvider> logger,
        IGraphQLClientFactory graphQLClientFactory,
        IGraphQLRequestBuilder graphQLRequestBuilder,
        IConfiguration configuration)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _graphQLClientFactory = graphQLClientFactory;
        _graphQLRequestBuilder = graphQLRequestBuilder;
        _configuration = configuration;
    }

    private static DotnetSitemapGenerator.ChangeFrequency GetChangeFrequency(string changefreq)
    {
        if (string.IsNullOrWhiteSpace(changefreq)) { return DotnetSitemapGenerator.ChangeFrequency.Never; }

        switch (changefreq.ToLowerInvariant())
        {
            case "always":
                return DotnetSitemapGenerator.ChangeFrequency.Always;
            case "daily":
                return DotnetSitemapGenerator.ChangeFrequency.Daily;
            case "weekly":
                return DotnetSitemapGenerator.ChangeFrequency.Weekly;
            case "yearly":
                return DotnetSitemapGenerator.ChangeFrequency.Yearly;
            case "hourly":
                return DotnetSitemapGenerator.ChangeFrequency.Hourly;
            case "never":
                return DotnetSitemapGenerator.ChangeFrequency.Never;
        }

        return DotnetSitemapGenerator.ChangeFrequency.Never;
    }

    public Task<IReadOnlyCollection<SitemapNode>> GetNodes()
    {
        var nodes = new List<SitemapNode>();
        var results = GetSiteMap();

        foreach (var result in results)
        {
            var url = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + result.Url.Path;
                
            _logger.LogInformation("Adding product: {URL}", url);

            SitemapNode node = new(url)
            {
                LastModificationDate = DateTime.ParseExact(result.UpdatedDatetime.Value,
                "yyyyMMdd'T'HHmmss'Z'",
                CultureInfo.InvariantCulture),
                Priority = Convert.ToDecimal(result.Priority.TargetItem.DisplayName),
                ChangeFrequency = GetChangeFrequency(result.ChangeFrequency.TargetItem.DisplayName)
            };
            nodes.Add(node);
        }

        return Task.FromResult<IReadOnlyCollection<SitemapNode>>(nodes);
    }

    private List<Result> GetSiteMap()
    {
        CustomGraphQlLayoutServiceHandler customGraphQlLayoutServiceHandler = new(_configuration, _graphQLRequestBuilder, _graphQLClientFactory);
        return  customGraphQlLayoutServiceHandler.GetSitemap().Result;
    }
}