using System.ComponentModel.DataAnnotations;

namespace MvpSite.Rendering.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class HttpsUrlAttribute()
    : ValidationAttribute("The field {0} must be a Url starting with 'https://'.")
{
    public override bool IsValid(object? value)
    {
        return value == null
               || (value.ToString()?.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ?? false);
    }
}