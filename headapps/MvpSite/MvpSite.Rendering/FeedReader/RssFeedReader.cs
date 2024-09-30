using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Extensions.Caching.Memory;
using MvpSite.Rendering.Models;

namespace MvpSite.Rendering.FeedReader;

public class RssFeedReader(IMemoryCache memoryCache, ILogger<RssFeedReader> logger) : IFeedReader
{
    public IList<FeedItem> GetFeed(string url)
    {
        return GetFeedItems(url, 60, 100);
    }

    private static List<FeedItem> LoadFeed(string url)
    {
        // Load the actual RSS feed
        XmlReader reader = XmlReader.Create(url);
        SyndicationFeed feed = SyndicationFeed.Load(reader);
        reader.Close();

        List<FeedItem> list = [];
        foreach (SyndicationItem item in feed.Items)
        {
            FeedItem feedItem = new()
            {
                Title = item.Title.Text,
                Description = item.Summary.Text,
                Url = item.Links.FirstOrDefault()?.Uri.ToString() ?? "#",
                Timestamp = item.PublishDate.DateTime
            };
            list.Add(feedItem);
        }

        return list;
    }

    private List<FeedItem> GetFeedItems(string url, int cacheInterval, int count)
    {
        string key = $"{url}_{count}_{cacheInterval}";
        string keyBackup = $"{url}_{count}_{cacheInterval}_backup";

        if (!memoryCache.TryGetValue(key, out IList<FeedItem>? feedItems))
        {
            feedItems = LoadFeed(url);

            if (feedItems.Any())
            {
                MemoryCacheEntryOptions mainCacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheInterval));
                MemoryCacheEntryOptions backupCatchEntryOptions = new() { Priority = CacheItemPriority.NeverRemove };

                memoryCache.Set(key, feedItems, mainCacheEntryOptions);
                memoryCache.Set(key, feedItems, backupCatchEntryOptions);
            }
            else if (!memoryCache.TryGetValue(keyBackup, out IList<FeedItem>? backupFeedItems))
            {
                logger.LogWarning("There was an problem with loading RSS feed items so getting them from backup cache, failed url: {url}", url);
                feedItems = backupFeedItems ?? [];
            }
        }

        return feedItems != null ? feedItems.Take(count).ToList() : [];
    }
}