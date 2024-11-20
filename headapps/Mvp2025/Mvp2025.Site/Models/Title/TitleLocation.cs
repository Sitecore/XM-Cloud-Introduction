using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models.Title;

public class TitleLocation
{
    public TitleUrl? Url { get; set; }
    public TitleField? Field { get; set; }
}