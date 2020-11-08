using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Helpers;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    /// <summary>
    /// Service for updating the cache
    /// </summary>
    public class PublicUpdatingCacheService : IPublicUpdatingCacheService
    {
        private readonly ICertificateRepository _repository;
        private readonly IMemoryCache _memoryCache;

        public PublicUpdatingCacheService(ICertificateRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public async Task SetItemAsync(int id, string userId)
        {
            var item = await _repository.GetCertificateIncludeLinksAsync(i => i.Id == id && i.UserId == userId);

            _memoryCache.Set(CacheHelper.GenerateCacheKey(nameof(CertificateViewModel), id.ToString()), item,
                             new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12)));
        }

        public void SetList(string userId)
        {
            _memoryCache.Set(CacheHelper.GenerateCacheKey(nameof(PublicViewModel), userId.Take(5).ToString()), _repository.List(i => i.UserId == userId).ToList(),
                             new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12)));
        }
    }
}
