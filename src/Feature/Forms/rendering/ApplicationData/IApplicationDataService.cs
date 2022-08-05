using Mvp.Feature.Forms.Models;
using System.Threading.Tasks;

namespace Mvp.Feature.Forms.ApplicationData
{
    public interface IApplicationDataService
    {
        public Task<ApplicationLists> GetApplicationListDataAsync();
    }
}
