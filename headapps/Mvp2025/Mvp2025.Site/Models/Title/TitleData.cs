using System.Text.Json.Serialization;

namespace Mvp2025.Site.Models.Title;

public class TitleData
{
    public TitleLocation? DataSource { get; set; }
    public TitleLocation? ContextItem { get; set; }
}