using System.Web;

namespace MvpSite.Rendering.Extensions;

public static class UriExtensions
{
    public static Uri? AddQueryString(this Uri? uri, string key, string value)
    {
        Uri? result = uri;
        if (uri != null)
        {
            int queryIndex = uri.OriginalString.IndexOf('?');
            result = queryIndex > 0
                ? new Uri(uri.OriginalString.Insert(queryIndex + 1, $"{key}={HttpUtility.UrlEncode(value)}&"), UriKind.RelativeOrAbsolute)
                : new Uri($"{uri.OriginalString}?{key}={HttpUtility.UrlEncode(value)}", UriKind.RelativeOrAbsolute);
        }

        return result;
    }

    public static Uri? AddSegment(this Uri? uri, string segment)
    {
        Uri? result = uri;
        if (uri != null)
        {
            result = uri.IsAbsoluteUri
                ? new Uri(uri, segment)
                : new Uri(uri.OriginalString.TrimEnd('/') + "/" + segment.TrimStart('/'), UriKind.Relative);
        }

        return result;
    }

    public static Uri? AddGravatarSizing(this Uri? uri, string size)
    {
        Uri? result = uri;
        if (uri != null && uri is { IsAbsoluteUri: true, Host: "www.gravatar.com" })
        {
            result = uri.AddQueryString("s", size).AddQueryString("d", "https://mvp.sitecore.net/images/mvp-base-user-grey.png");
        }

        return result;
    }
}