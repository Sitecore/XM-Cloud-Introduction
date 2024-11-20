using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models;

public class LinkListItemField
{
    [JsonPropertyName("link")]
    public HyperLinkField? Link { get; set; }
}