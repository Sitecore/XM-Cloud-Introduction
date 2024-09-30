namespace MvpSite.Rendering.Models;

public class FeedItem
{
    public string Description { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = "#";
}