using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IEventViewModelService
    {
        Task<EventListViewModel> GetEventListAsync(int page);
    }
}
