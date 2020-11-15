using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Helpers;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class PublicViewModelService : IPublicViewModelService
    {
        private readonly ICertificateRepository _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public PublicViewModelService(ICertificateRepository repository, IMemoryCache memoryCache, IStringLocalizer<SharedResource> localizer)
        {
            _repository = repository;
            _memoryCache = memoryCache;
            _localizer = localizer;
        }

        public PublicViewModel GetPublicViewModel(string year, string find, string userId, string name, string middleName, string surname, string country, string code, byte[] photo)
        {
            if (!_memoryCache.TryGetValue(CacheHelper.GenerateCacheKey(nameof(PublicViewModel), userId.Take(5).ToString()), out List<Certificate> items))
            {
                items = _repository.List(i => i.UserId == userId).ToList();

                if (items != null)
                {
                    _memoryCache.Set(CacheHelper.GenerateCacheKey(nameof(PublicViewModel), userId.Take(5).ToString()), items,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12)));
                }
            }

            //Sort by date
            if (year != null && year != "Все")
            {
                items = items.Where(i => i.Date.Year == int.Parse(year)).ToList();
            }

            //Sort by title
            if (!string.IsNullOrEmpty(find))
            {
                items = items.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower())).ToList();
            }

            var certificates = items.Select(i =>
            {
                var certificateViewModel = new CertificateViewModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Date = i.Date,
                    ImageData = i.File,
                    UserId = i.UserId
                };
                return certificateViewModel;
            });

            List<string> years = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse().Select(i => i.ToString()).ToList();
            years.Insert(0, _localizer["All"].Value);

            PublicViewModel pvm = new PublicViewModel
            {
                Certificates = certificates,
                Find = find,
                Years = new SelectList(years),
                Year = year,
                Name = name,
                Country = country,
                Surname = surname,
                UniqueUrl = code,
                MiddleName = middleName,
                ImageData = photo
            };

            return pvm;
        }

        public async Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int id, string userId, string url)
        {
            if (!_memoryCache.TryGetValue(CacheHelper.GenerateCacheKey(nameof(CertificateViewModel), id.ToString()), out Certificate certificate))
            {
                certificate = await _repository.GetCertificateIncludeLinksAsync(i => i.Id == id && i.UserId == userId);

                if (certificate != null)
                {
                    _memoryCache.Set(CacheHelper.GenerateCacheKey(nameof(CertificateViewModel), id.ToString()), certificate,
                                     new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12)));
                }
            }

            return new CertificateViewModel
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Description = certificate.Description,
                Links = certificate.Links,
                Stage = certificate.Stage,
                Date = certificate.Date,
                UniqueUrl = url,
                ImageData = certificate.File,
                UserId = certificate.UserId
            };
        }
    }
}
