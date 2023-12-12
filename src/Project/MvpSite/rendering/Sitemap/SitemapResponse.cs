using System.Text.Json;

namespace Mvp.Project.MvpSite.Sitemap
{
    public class SitemapQueryResponse
    {
            //
            // Summary:
            //     Gets or sets Layout Service GraphQL Response.
          public SitemapModel SiteMap { get; set; }
    }

    public class SitemapModel
    {
        //
        // Summary:
        //     Gets or sets Layout Service GraphQL Response.
        public JsonElement? Sitemap { get; set; }
    }
}
