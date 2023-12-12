using DotnetSitemapGenerator;
using DotnetSitemapGenerator.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Rendering;

public class SitemapBuilder
{
    private readonly IEnumerable<ISitemapUrlProvider> _providers;

    public SitemapBuilder(IEnumerable<ISitemapUrlProvider> providers)
    {
        _providers = providers;
    }

    public async Task<MemoryStream> GenerateAsync()
    {
        var nodes = new List<SitemapNode>();
        foreach (var provider in _providers)
        {
            nodes.AddRange(await provider.GetNodes());
        }

        IXmlSerializer sitemapProvider = new XmlSerializer();
        var memory = new MemoryStream();
        sitemapProvider.SerializeToStream(new SitemapModel(nodes), memory);
        // reset the stream to the start
        memory.Position = 0;
        return memory;
    }
}