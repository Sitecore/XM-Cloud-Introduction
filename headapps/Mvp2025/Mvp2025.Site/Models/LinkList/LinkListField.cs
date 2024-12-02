using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models;

public class LinkListField
{
    [JsonPropertyName("title")]
    public TextField? Title { get; set; }
}