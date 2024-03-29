﻿using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services;

public class CachedPublicViewModelService : ICachedPublicViewModelService
{
    private readonly ICertificateRepository _repository;
    private readonly IMemoryCache _memoryCache;

    public CachedPublicViewModelService(ICertificateRepository repository, IMemoryCache memoryCache)
    {
        _repository = repository;
        _memoryCache = memoryCache;
    }


    public async Task<Certificate?> GetItemAsync(int id, string userId)
    {
        return await _memoryCache.GetOrCreateAsync(nameof(CertificateViewModel).GenerateCacheKey(id.ToString()),
                                                   async item =>
                                                   {
                                                       item.SlidingExpiration = CacheHelper.DefaultExpiration;

                                                       return await _repository.GetCertificateIncludeLinksAsync(id, userId);
                                                   });
    }


    public List<Certificate> GetList(string userId)
    {
        return _memoryCache.GetOrCreate(nameof(PublicViewModel).GenerateCacheKey(userId.ToString()), item =>
        {
            item.SlidingExpiration = CacheHelper.DefaultExpiration;

            return _repository.ListByUserId(userId).ToList();
        });
    }


    public async Task SetItemAsync(int id, string userId)
    {
        var item = await _repository.GetCertificateIncludeLinksAsync(id, userId);

        _memoryCache.Set(nameof(CertificateViewModel).GenerateCacheKey(id.ToString()), item,
                         new MemoryCacheEntryOptions().SetSlidingExpiration(CacheHelper.DefaultExpiration));
    }


    public void SetList(string userId)
    {
        _memoryCache.Set(nameof(PublicViewModel).GenerateCacheKey(userId.ToString()),
                         _repository.ListByUserId(userId).ToList(),
                         new MemoryCacheEntryOptions().SetSlidingExpiration(CacheHelper.DefaultExpiration));
    }
}
