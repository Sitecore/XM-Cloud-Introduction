namespace MvpSite.Rendering.Extensions;

// ReSharper disable once InconsistentNaming - This class extends the interface, not the type
public static class ICollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            collection.Add(item);
        }
    }
}