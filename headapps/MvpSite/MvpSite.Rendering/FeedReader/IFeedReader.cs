using MvpSite.Rendering.Models;

namespace MvpSite.Rendering.FeedReader;

public interface IFeedReader
{
    public IList<FeedItem> GetFeed(string url);
}