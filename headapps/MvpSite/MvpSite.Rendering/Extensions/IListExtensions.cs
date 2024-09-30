namespace MvpSite.Rendering.Extensions;

// ReSharper disable once InconsistentNaming - This class extends the interface, not the type
public static class IListExtensions
{
    public static string ToCommaSeparatedStringOrNullLiteral<T>(this IList<T>? list)
    {
        return list != null ? string.Join(',', list) : "null";
    }
}