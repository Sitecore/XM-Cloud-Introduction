using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models;

public class LinkListItem 
{
    [JsonPropertyName("field")]
    public LinkListItemField? Field { get; set; }
}