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
    public class PublicViewModelService : IPublicViewModelService
    {
        private readonly ICertificateRepository _repository;
        private readonly IMemoryCache _memoryCache;

        public PublicViewModelService(ICertificateRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public PublicViewModel GetPublicViewModel(string year, string find, string userId, string name, string middleName, string surname, string country, string code, byte[] photo)
        {
            var items = _repository.List(i => i.UserId == userId);

            if (year != null && year != "Все")
            {
                items = items.Where(i => i.Date.Year == int.Parse(year));
            }

            if (!string.IsNullOrEmpty(find))
            {
                items = items.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower()));
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

            PublicViewModel pvm = new PublicViewModel
            {
                Certificates = certificates,
                Find = find,
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
            if (!_memoryCache.TryGetValue(id, out Certificate certificate))
            {
                certificate = await _repository.GetCertificateIncludeLinksAsync(i => i.Id == id && i.UserId == userId);
                if (certificate != null)
                {
                    _memoryCache.Set(certificate.Id, certificate,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
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
