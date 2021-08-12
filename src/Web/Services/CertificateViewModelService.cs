using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Linq;
using System.Threading;
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
        private readonly IStageService _stageService;

        public CertificateViewModelService(ICertificateRepository repository,
                                           IStringLocalizer<SharedResource> localizer,
                                           ICachedPublicViewModelService cacheService,
                                           IStageService stageService)
        {
            _repository = repository;
            _localizer = localizer;
            _cacheService = cacheService;
            _stageService = stageService;
        }


        public IndexViewModel GetIndexViewModel(int page, string userId, string year, string find, Stage? stage)
        {
            page = page <= 0 ? 1 : page;

            var list = _repository.ListByUserId(userId);

            // Sort by date.
            if (year != null && year != _localizer["All"] && int.TryParse(year, out int result))
            {
                list = list.Where(i => i.Date.Year == result);
            }

            // Sort by title.
            if (!string.IsNullOrEmpty(find))
            {
                list = list.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower()));
            }

            // Sort by stage.
            if (stage != null)
            {
                list = list.Where(p => p.Stage == stage);
            }

            var count = list.Count();
            var skipped = (page - 1) * Common.PageSize;

            if (skipped >= count && count != 0)
            {
                page -= 1;
                skipped -= Common.PageSize;
            }

            var items = list.Skip(skipped).Take(Common.PageSize);

            var certificates = items.Select(i =>
            {
                var certificateViewModel = new CertificateViewModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Date = i.Date,
                    ImageData = i.File
                };

                return certificateViewModel;
            });

            var years = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse().Select(i => i.ToString()).ToList();
            years.Insert(0, _localizer["All"].Value);

            var stages = _stageService.GetStages();
            stages.Insert(0, new() { EnumName = string.Empty, Name = _localizer["All"].Value });

            IndexViewModel ivm = new IndexViewModel
            {
                Certificates = certificates,
                Find = find,
                Year = year,
                Stage = null,
                Years = new SelectList(years),
                Stages = new SelectList(stages, "EnumName", "Name"),
                Controller = "Certificate",
                PageViewModel = new PageViewModel(count, page, Common.PageSize)
            };

            return ivm;
        }


        public async Task CreateCertificateAsync(CertificateViewModel cvm, 
                                                 string userId, 
                                                 CancellationToken cancellationToken = default)
        {
            await _repository.CreateAsync(new Certificate
            (
                userId,
                cvm.Title,
                cvm.ImageData,
                cvm.Description,
                cvm.Stage,
                cvm.Date
            ), cancellationToken);

            _cacheService.SetList(userId);
        }


        public async Task UpdateCertificateAsync(CertificateViewModel cvm, 
                                                 string userId, 
                                                 CancellationToken cancellationToken = default)
        {
            var certificate = new Certificate
            (
                userId,
                cvm.Title,
                cvm.ImageData,
                cvm.Description,
                cvm.Stage,
                cvm.Date
            ).SetId(cvm.Id);

            await _repository.UpdateAsync(certificate, cancellationToken);

            await _cacheService.SetItemAsync(certificate.Id, userId);
            _cacheService.SetList(userId);
        }


        public async Task DeleteCertificateAsync(int id, 
                                                 string userId, 
                                                 CancellationToken cancellationToken = default)
        {
            var certificate = await _repository.GetByUserIdAsync(id, userId, cancellationToken);

            await _repository.DeleteAsync(certificate, cancellationToken);

            _cacheService.SetList(userId);
        }


        public async Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int page, 
                                                                                    int id, 
                                                                                    string userId, 
                                                                                    CancellationToken cancellationToken = default)
        {
            var certificate = await _repository.GetCertificateIncludeLinksAsync(id, userId, cancellationToken);

            if (certificate is null)
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


        public async Task<CertificateViewModel> GetCertificateByIdAsync(int id, 
                                                                        string userId, 
                                                                        CancellationToken cancellationToken = default)
        {
            var certificate = await _repository.GetByUserIdAsync(id, userId, cancellationToken);

            if (certificate is null)
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
                File = new FormFile(new MemoryStream(certificate.File), 
                                    0, 
                                    certificate.File.Length, 
                                    "File", 
                                    "File"),
                ImageData = certificate.File
            };
        }
    }
}
