using Mvp.Feature.People.Models;
using System.Threading.Tasks;

namespace Mvp.Feature.People.PeopleFinder
{
    public interface IPeopleFinder
    {
        public Task<PeopleSearchResults> FindPeople(SearchParams searchParams);
    }
}