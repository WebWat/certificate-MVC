using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class ModeratorViewModelService : IModeratorViewModelService
    {
        private readonly IAsyncRepository<Event> _repository;
        private readonly IMemoryCache _memoryCache;

        public ModeratorViewModelService(IAsyncRepository<Event> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public async Task CreateEventAsync(EventViewModel evm)
        {
            await _repository.CreateAsync(new Event
            {
                Title = evm.Title,
                Description = evm.Description,
                Url = evm.Url,
                Date = DateTime.UtcNow
            });
        }

        public async Task DeleteEventAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(item);
        }

        public async Task<EventViewModel> GetEventByIdAsync(int id, int page)
        {
            var item = await _repository.GetByIdAsync(id);

            return new EventViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Url = item.Url,
                Date = item.Date,
                Page = page
            };
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
                        Id = i.Id,
                        Title = i.Title,
                        Description = i.Description,
                        Url = i.Url,
                        Date = i.Date,
                        Page = page
                    };

                    return item;
                }),
                PageViewModel = new PageViewModel(count, page, pageSize)
            };
        }

        public async Task UpdateEventAsync(EventViewModel evm)
        {
            await _repository.UpdateAsync(new Event
            {
                Id = evm.Id,
                Title = evm.Title,
                Description = evm.Description,
                Url = evm.Url,
                Date = evm.Date
            });
        }
    }
}
