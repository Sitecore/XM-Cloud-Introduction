using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Properties;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace Mvp2025.Site.Models.Title;

public class Title : BaseModel
{
    [SitecoreComponentField(Name = "data")]            
    public TitleData? Data { get; set; }

    public HyperLinkField Link 
    { 
        get 
        {
            return new HyperLinkField(
                    new HyperLink
                    {
                        Anchor = TitleLocation?.Url?.Path,
                        Title = TitleLocation?.Field?.JsonValue?.Value
                    });
        }
    }

    public TextField Text
    {
        get
        {   
            return new TextField(TitleLocation?.Field?.JsonValue?.Value ?? string.Empty);
        }
    }


    public TitleLocation? TitleLocation
    {
        get
        {
            return Data?.DataSource ?? Data?.ContextItem;                
        }
    }
}