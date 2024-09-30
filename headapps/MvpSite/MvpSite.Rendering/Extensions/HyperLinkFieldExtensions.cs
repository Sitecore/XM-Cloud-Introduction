using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Extensions;

public static class HyperLinkFieldExtensions
{
    public static Uri? ToUri(this HyperLinkField field)
    {
        Uri? result = null;
        if (!string.IsNullOrWhiteSpace(field.Value.Href))
        {
            result = new Uri(field.Value.Href, UriKind.RelativeOrAbsolute);
        }

        return result;
    }
}