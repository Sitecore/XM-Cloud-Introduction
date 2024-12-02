using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;
using System.Text.RegularExpressions;

namespace Mvp2025.Site.Models;

public partial class Container : BaseModel
{
    [GeneratedRegex("/mediaurl=\\\"([^\"]*)\\\"/", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex MediaUrlPattern();

    [SitecoreComponentParameter(Name = "BackgroundImage")]
    public string? BackgroundImage { get; set; }

    public string? BackgroundStyle
    { 
        get
        {
            if (!string.IsNullOrEmpty(BackgroundImage) && MediaUrlPattern().IsMatch(BackgroundImage))
            {
                string mediaUrl = MediaUrlPattern().Match(BackgroundImage).Groups[1].Value;
                return $"backgroundImage: url('{mediaUrl}')";
            }

            return string.Empty;
        }
    }

    public bool OutputContainerWrapper 
    {
        get
        {
            return Styles?.Split(' ').Any(x => x == "container") == true;
        }
    }
}
