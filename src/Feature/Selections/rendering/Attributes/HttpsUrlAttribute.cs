using System;
using System.ComponentModel.DataAnnotations;

namespace Mvp.Feature.Selections.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class HttpsUrlAttribute()
        : ValidationAttribute("The field {0} must be a Url starting with 'https'.")
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return value is string valueAsString &&
                   valueAsString.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }
    }
}
