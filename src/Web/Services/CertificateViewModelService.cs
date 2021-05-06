using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICachedPublicViewModelService _cacheService;

        public CertificateViewModelService(ICertificateRepository repository,
                                           IStringLocalizer<SharedResource> localizer,
                                           ICachedPublicViewModelService cacheService)
        {
            _repository = repository;
            _localizer = localizer;
            _cacheService = cacheService;
        }

        public IndexViewModel GetIndexViewModel(int page, string userId, string year, string find)
        {
            page = page <= 0 ? 1 : page;

            var list = _repository.ListByUserId(userId);

            //Sort by date
            if (year != null && year != _localizer["All"])
            {
                list = list.Where(i => i.Date.Year == int.Parse(year));
            }

            //Sort by title
            if (!string.IsNullOrEmpty(find))
            {
                list = list.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower()));
            }

            int pageSize = 12;

            var count = list.Count();
            var items = list.Skip((page - 1) * pageSize).Take(pageSize);

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
            });

            List<string> years = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse().Select(i => i.ToString()).ToList();
            years.Insert(0, _localizer["All"].Value);

            IndexViewModel ivm = new IndexViewModel
            {
                Certificates = certificates,
                Find = find,
                Year = year,
                Years = new SelectList(years),
                PageViewModel = new PageViewModel(count, page, pageSize)
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

            await _cacheService.SetItemAsync(certificate.Id, userId);
            _cacheService.SetList(userId);
        }

        public async Task DeleteCertificateAsync(int id, string userId)
        {
            var certificate = await _repository.GetByUserIdAsync(id, userId);

            await _repository.DeleteAsync(certificate);

            _cacheService.SetList(userId);
        }

        public async Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int page, int id, string userId)
        {
            var certificate = await _repository.GetCertificateIncludeLinksAsync(id, userId);

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
                UserId = certificate.UserId,
                Page = page
            };
        }

        public async Task<CertificateViewModel> GetCertificateByIdAsync(int id, string userId)
        {
            var certificate = await _repository.GetByUserIdAsync(id, userId);

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
