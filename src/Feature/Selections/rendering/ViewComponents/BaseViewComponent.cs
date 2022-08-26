using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Sitecore.AspNet.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents
{
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

        protected async Task<string> GetCurrentTokenAsync()
        {
            string result = await HttpContext.GetTokenAsync("id_token");
            return result;
        }
    }
}
