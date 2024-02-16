namespace Mvp.Foundation.DataFetching.GraphQL.Models
{
    public class NameItem
    {
        public string Name { get; set; }
    }

    public class ValueItem
    {
        public string Value { get; set; }
    }

    public class TargetItemFieldValueItem
    {
        public FieldValueItem TargetItem { get; set; }
    }

    public class FieldValueItem
    {
        public ValueItem Field { get; set; }
    }

    public class TargetItem
    {
        public string Name { get; set; }
    }

    public class PageInfo
    {
        public string endCursor { get; set; }
        public bool hasNextPage { get; set; }
    }

    public class Template
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class BoolField
    {
        public bool BoolValue { get; set; }
    }
}
