using DotnetSitemapGenerator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Rendering;

public interface ISitemapUrlProvider
{
    Task<IReadOnlyCollection<SitemapNode>> GetNodes();
}