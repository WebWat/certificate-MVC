using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class PublicViewModelService : IPublicViewModelService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICachedPublicViewModelService _cacheService;
        private readonly IStageService _stageService;

        public PublicViewModelService(ICachedPublicViewModelService cacheService, IStringLocalizer<SharedResource> localizer,
                                      IStageService stageService)
        {
            _cacheService = cacheService;
            _localizer = localizer;
            _stageService = stageService;
        }

        public PublicViewModel GetPublicViewModel(int page, string year, string find, Stage? stage, 
                                                  string userId, string name, string middleName, 
                                                  string surname, string code, byte[] photo)
        {
            page = page <= 0 ? 1 : page;

            var list = _cacheService.GetList(userId);

            //Sort by date
            if (year != null && year != _localizer["All"])
            {
                list = list.Where(i => i.Date.Year == int.Parse(year)).ToList();
            }

            //Sort by title
            if (!string.IsNullOrEmpty(find))
            {
                list = list.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower())).ToList();
            }

            //Sort by stage
            if (stage != null)
            {
                list = list.Where(p => p.Stage == stage).ToList();
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
                    ImageData = i.File,
                    UserId = i.UserId
                };
                return certificateViewModel;
            });

            var years = Enumerable.Range(2000, DateTime.Now.Year - 1999).Reverse().Select(i => i.ToString()).ToList();
            years.Insert(0, _localizer["All"].Value);

            var stages = _stageService.GetStages();
            stages.Insert(0, new() { EnumName = string.Empty, Name = _localizer["All"].Value });

            PublicViewModel pvm = new PublicViewModel
            {
                Certificates = certificates,
                Find = find,
                Stage = null,
                Year = year,
                Stages = new SelectList(stages, "EnumName", "Name"),
                Years = new SelectList(years),
                Name = name,
                Surname = surname,
                UniqueUrl = code,
                MiddleName = middleName,
                ImageData = photo,
                PageViewModel = new PageViewModel(count, page, pageSize)
            };

            return pvm;
        }

        public async Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int page, int id, string userId, string url)
        {
            var certificate = await _cacheService.GetItemAsync(id, userId);

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
                UserId = certificate.UserId,
                Page = page
            };
        }
    }
}
