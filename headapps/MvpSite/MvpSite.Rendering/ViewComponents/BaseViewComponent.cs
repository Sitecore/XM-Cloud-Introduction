using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents;

public abstract class BaseViewComponent : ViewComponent
{
    protected BaseViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    {
        ModelBinder = modelBinder;
        Client = client;
    }

    protected IViewModelBinder ModelBinder { get; }

    protected MvpSelectionsApiClient Client { get; }

    public abstract Task<IViewComponentResult> InvokeAsync();
}