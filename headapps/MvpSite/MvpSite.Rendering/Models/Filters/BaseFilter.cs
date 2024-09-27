namespace MvpSite.Rendering.Models.Filters;

public abstract class BaseFilter(string? prefix)
{
    protected string Prefix { get; set; } = string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix.EndsWith('.') ? prefix : prefix + ".";

    public abstract string ToQueryString(bool isFirst = false);

    public class None() : BaseFilter(null)
    {
        public override string ToQueryString(bool isFirst = false)
        {
            return string.Empty;
        }
    }
}