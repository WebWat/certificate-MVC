using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Helpers;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class EventViewModelService : IEventViewModelService
    {
        private readonly IAsyncRepository<Event> _repository;
        private readonly IMemoryCache _memoryCache;

        public EventViewModelService(IAsyncRepository<Event> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public async Task<EventListViewModel> GetEventListAsync(int page)
        {
            if (!_memoryCache.TryGetValue(CacheHelper.GenerateCacheKey(nameof(PublicViewModel), "0"), out IEnumerable<Event> items))
            {
                items = await _repository.ListAllAsync();

                if (items != null)
                {
                    _memoryCache.Set(CacheHelper.GenerateCacheKey(nameof(PublicViewModel), "0"), items,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                }
            }

            int pageSize = 6;

            var count = items.Count();
            items = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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
