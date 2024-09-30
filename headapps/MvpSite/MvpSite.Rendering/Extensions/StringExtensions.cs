namespace MvpSite.Rendering.Extensions;

public static class StringExtensions
{
    public static string DecodeSpaces(this string s)
    {
        return s
            .Replace("+", " ")
            .Replace("-", " ")
            .Replace("%20", " ");
    }

    public static string EncodeSpaces(this string s)
    {
        return s.Replace(' ', '-');
    }
}