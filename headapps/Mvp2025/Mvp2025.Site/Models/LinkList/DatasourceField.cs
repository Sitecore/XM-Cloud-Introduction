using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models;

public class DatasourceField
{
    [JsonPropertyName("field")]
    public LinkListField? Field { get; set; }

    [JsonPropertyName("children")]
    public LinkListChildren? Children { get; set; }
}