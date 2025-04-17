namespace MvpSite.Rendering.Extensions;

public static class BooleanExtensions
{
    public static string ToStringOrNullLiteral(this bool? boolean)
    {
        string result = "null";
        if (boolean.HasValue)
        {
            result = boolean.Value ? "true" : "false";
        }

        return result;
    }
}