namespace Mvp.Feature.People.Extensions
{
    public static class StringExtensions
    {
        public static string DecodeSpaces(this string s)
        {
            return s
                .Replace("+", " ")
                .Replace("-", " ")
                .Replace("%20", " ");
        }
    }
}
