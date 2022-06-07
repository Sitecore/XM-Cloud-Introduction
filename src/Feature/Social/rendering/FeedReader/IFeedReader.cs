using Mvp.Feature.Social.Models;
using System.Collections.Generic;

namespace Mvp.Feature.Social.FeedReader
{
    public interface IFeedReader
    {
        public IList<FeedItem> GetFeed(string url);
    }
}
