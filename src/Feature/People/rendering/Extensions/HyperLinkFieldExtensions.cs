using System;
using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.People.Extensions
{
    public static class HyperLinkFieldExtensions
    {
        public static Uri ToUri(this HyperLinkField field)
        {
            Uri result = null;
            if (!string.IsNullOrWhiteSpace(field.Value.Href))
            {
                result = new Uri(field.Value.Href, UriKind.RelativeOrAbsolute);
            }

            return result;
        }
    }
}
