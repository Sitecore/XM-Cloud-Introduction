using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mvp.Feature.Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Mvp.Feature.Social.FeedReader
{
    public class RssFeedReader : IFeedReader
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<RssFeedReader> logger;

        public RssFeedReader(IMemoryCache memoryCache, ILogger<RssFeedReader> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public IList<FeedItem> GetFeed(string url)
        {
            return GetFeedItems(url, 60, 100);
        }

        public IList<FeedItem> GetFeedItems(string url, int cacheInterval, int count)
        {
            string key = $"{url}_{count}_{cacheInterval}";
            string key_backup = $"{url}_{count}_{cacheInterval}_backup";

            if (!memoryCache.TryGetValue(key, out IList<FeedItem> feedItems))
            {
                feedItems = this.LoadFeed(url);

                if(feedItems.Any())
                {
                    var mainCacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheInterval));
                    var backupCatchEntryOptions = new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove };

                    memoryCache.Set(key, feedItems, mainCacheEntryOptions);
                    memoryCache.Set(key, feedItems, backupCatchEntryOptions);
                }
                else if (!memoryCache.TryGetValue(key_backup, out IList<FeedItem> backupFeedItems))
                {
                    logger.LogWarning($"There was an problem with loading RSS feed items so getting them from backup cache, failed url: {url}");
                    feedItems = backupFeedItems;
                }               
            }

            return feedItems.Take(count).ToList();
        }

        private IList<FeedItem> LoadFeed(string url)
        {
            // Load the actual RSS feed
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            var list = new List<FeedItem>();
            foreach (SyndicationItem item in feed.Items)
            {
                var feedItem = new FeedItem()
                {
                    Title = item.Title.Text,
                    Description = item.Summary.Text,
                    Url = item.Links.FirstOrDefault().Uri.ToString(),
                    Timestamp = item.PublishDate.DateTime
                };
                list.Add(feedItem);
            }
            return list;
        }
    }
}
