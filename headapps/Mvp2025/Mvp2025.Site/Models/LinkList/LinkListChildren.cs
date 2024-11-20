using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models;

public class LinkListChildren
{
    [JsonPropertyName("results")]
    public List<LinkListItem>? Results { get; set; }
}