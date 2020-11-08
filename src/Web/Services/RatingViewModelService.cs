using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Helpers;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class RatingViewModelService : IRatingViewModelService
    {
        private readonly IRatingService _ratingService;
        private readonly IMemoryCache _memoryCache;

        public RatingViewModelService(IRatingService ratingService, IMemoryCache memoryCache)
        {
            _ratingService = ratingService;
            _memoryCache = memoryCache;
        }

        public IEnumerable<UserViewModel> GetTopTenUsers()
        {
            if (!_memoryCache.TryGetValue(CacheHelper.GenerateCacheKey(nameof(UserViewModel)), out IEnumerable<UserViewModel> items))
            {
                items = _ratingService.GetTopTen().Select(i =>
                {
                    var user = new UserViewModel
                    {
                        Name = i.Name,
                        Country = i.Country,
                        Rating = i.Rating
                    };

                    return user;
                });

                if (items != null)
                {
                    _memoryCache.Set(CacheHelper.GenerateCacheKey(nameof(UserViewModel)), items,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }

            return items;
        }
    }
}
