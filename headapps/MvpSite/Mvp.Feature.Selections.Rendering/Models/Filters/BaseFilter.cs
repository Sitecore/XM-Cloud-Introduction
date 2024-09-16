namespace Mvp.Feature.Selections.Models.Filters
{
    public abstract class BaseFilter
    {
        protected BaseFilter(string prefix)
        {
            Prefix = string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix.EndsWith(".") ? prefix : prefix + ".";
        }

        public string Prefix { get; set; }

        public abstract string ToQueryString(bool isFirst = false);

        public class None : BaseFilter
        {
            public None()
                : base(null)
            {
            }

            public override string ToQueryString(bool isFirst = false)
            {
                return string.Empty;
            }
        }
    }
}
