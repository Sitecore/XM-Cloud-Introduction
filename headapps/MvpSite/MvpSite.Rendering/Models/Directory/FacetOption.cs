namespace MvpSite.Rendering.Models.Directory;

public class FacetOption
{
    public required string Identifier { get; set; }

    public string? Display { get; set; }

    public int Count { get; set; }

    public bool IsActive { get; set; }
}