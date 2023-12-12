// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

public class ChangeFrequency
{
    public TargetItem TargetItem { get; set; }
}

public class SitemapData
{
    public SearchData Search { get; set; }
}

public class PageInfo
{
    public string endCursor { get; set; }
    public bool hasNext { get; set; }
}

public class Priority
{
    public TargetItem TargetItem { get; set; }
}

public class Result
{
    public Updateddatetime UpdatedDatetime { get; set; }
    public Url Url { get; set; }
    public string Name { get; set; }
    public Priority Priority { get; set; }
    public ChangeFrequency ChangeFrequency { get; set; }
}

public class Root
{
    public SitemapData Data { get; set; }
}

public class SearchData
{
    public int total { get; set; }
    public PageInfo pageInfo { get; set; }
    public List<Result> Results { get; set; }
}

public class TargetItem
{
    public string DisplayName { get; set; }
}

public class Updateddatetime
{
    public string Value { get; set; }
}

public class Url
{
    public string Path { get; set; }
}

