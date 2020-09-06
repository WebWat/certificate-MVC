using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class EventViewModelService : IEventViewModelService
    {
        private readonly IAsyncRepository<Event> _repository;

        public EventViewModelService(IAsyncRepository<Event> repository)
        {
            _repository = repository;
        }

        public async Task<EventListViewModel> GetEventListAsync(int page)
        {
            var list = await _repository.ListAllAsync();

            int pageSize = 6;

            var count = list.Count();
            var items = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new EventListViewModel
            {
                Events = items.Select(i =>
                {
                    var item = new EventViewModel
                    {
                        Title = i.Title,
                        Description = i.Description,
                        Url = i.Url,
                        Date = i.Date
                    };

                    return item;
                }),
                PageViewModel = new PageViewModel(count, page, pageSize)
            };
        }
    }
}
