using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IModeratorViewModelService
    {
        Task<EventListViewModel> GetEventListAsync(int page);
        Task UpdateEventAsync(EventViewModel evm);
        Task CreateEventAsync(EventViewModel evm);
        Task DeleteEventAsync(int id);
        Task<EventViewModel> GetEventByIdAsync(int id, int page);
    }
}
