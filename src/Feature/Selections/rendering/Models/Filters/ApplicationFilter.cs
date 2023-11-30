using System;
using Mvp.Selections.Domain;

namespace Mvp.Feature.Selections.Models.Filters
{
    public class ApplicationFilter : BaseFilter
    {
        public ApplicationFilter(string prefix)
            : base(prefix)
        {
        }

        public Guid? SelectionId { get; set; }

        public short? CountryId { get; set; }

        public string ApplicantName { get; set; }

        public ApplicationStatus? Status { get; set; }

        public override string ToQueryString(bool isFirst = false)
        {
            char first = isFirst ? '?' : '&';
            return $"{first}{Prefix}{nameof(SelectionId)}={SelectionId}&{Prefix}{nameof(CountryId)}={CountryId}&{Prefix}{nameof(ApplicantName)}={ApplicantName}&{Prefix}{nameof(Status)}={Status}";
        }
    }
}
