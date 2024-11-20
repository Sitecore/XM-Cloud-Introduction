using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models;

public class DataField
{
    [JsonPropertyName("datasource")]
    public DatasourceField? Datasource { get; set; }
}