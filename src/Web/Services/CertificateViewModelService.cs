using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class CertificateViewModelService : ICertificateViewModelService
    {
        private readonly ICertificateRepository _repository;
        private readonly IPublicUpdatingCacheService _cacheService;

        public CertificateViewModelService(ICertificateRepository repository, IPublicUpdatingCacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public IndexViewModel GetIndexViewModel(string userId, string year, string find)
        {
            var items = _repository.List(i => i.UserId == userId).ToList();

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
                    Stage = i.Stage,
                    ImageData = i.File
                };

                return certificateViewModel;
            }).ToList();

            List<string> years = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse().Select(i => i.ToString()).ToList();
            years.Insert(0, "Все");

            IndexViewModel ivm = new IndexViewModel
            {
                Certificates = certificates,
                Find = find,
                Year = year,
                Years = new SelectList(years),
            };

            return ivm;
        }

        public async Task CreateCertificateAsync(CertificateViewModel cvm, string userId)
        {
            await _repository.CreateAsync(new Certificate
            {
                Title = cvm.Title,
                Description = cvm.Description,
                Date = cvm.Date,
                Stage = cvm.Stage,
                File = cvm.ImageData,
                UserId = userId
            });

            _cacheService.SetList(userId);
        }

        public async Task UpdateCertificateAsync(CertificateViewModel cvm, string userId)
        {
            var certificate = new Certificate
            {
                Id = cvm.Id,
                Title = cvm.Title,
                Description = cvm.Description,
                Date = cvm.Date,
                Stage = cvm.Stage,
                UserId = userId
            };

            //If the file remains the same
            if (cvm.ImageData == null)
            {
                var update = await _repository.GetByIdAsync(cvm.Id);
                certificate.File = update.File;
                await _repository.UpdateAsync(certificate);
            }
            else
            {
                certificate.File = cvm.ImageData;
                await _repository.UpdateAsync(certificate);
            }

            await _cacheService.SetItemAsync(cvm.Id, userId);

            _cacheService.SetList(userId);
        }

        public async Task DeleteCertificateAsync(int id, string userId)
        {
            var certificate = await _repository.GetAsync(i => i.Id == id && i.UserId == userId);

            await _repository.DeleteAsync(certificate);

            _cacheService.SetList(userId);
        }

        public async Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int id, string userId)
        {
            var certificate = await _repository.GetCertificateIncludeLinksAsync(i => i.Id == id && i.UserId == userId);

            if (certificate == null)
            {
                return null;
            }

            return new CertificateViewModel
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Description = certificate.Description,
                Links = certificate.Links,
                Date = certificate.Date,
                Stage = certificate.Stage,
                ImageData = certificate.File,
                UserId = certificate.UserId
            };
        }

        public async Task<CertificateViewModel> GetCertificateByIdAsync(int id, string userId)
        {
            var certificate = await _repository.GetAsync(i => i.Id == id && i.UserId == userId);

            if (certificate == null)
            {
                return null;
            }

            return new CertificateViewModel
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Description = certificate.Description,
                Date = certificate.Date,
                Stage = certificate.Stage,
                File = new FormFile(new MemoryStream(certificate.File), 0, certificate.File.Length, "File", "File"),
                ImageData = certificate.File
            };
        }
    }
}
