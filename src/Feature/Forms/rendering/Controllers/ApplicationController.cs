using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Forms.ApplicationData;
using System.Threading.Tasks;

namespace Mvp.Feature.Forms.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationDataService applicationDataService;

        public ApplicationController(IApplicationDataService applicationDataService)
        {
            this.applicationDataService = applicationDataService;
        }

        public async Task<IActionResult> GetApplicationLists()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            return Json(await applicationDataService.GetApplicationListDataAsync());
        }

        public IActionResult GetApplicationInfo()
        {
            return Json(new 
            {
                UserIsAuthenticated = User.Identity.IsAuthenticated
            });
        }
    }
}
