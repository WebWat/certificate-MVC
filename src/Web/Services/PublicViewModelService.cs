using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
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

        public PublicViewModelService(ICachedPublicViewModelService cacheService, 
                                      IStringLocalizer<SharedResource> localizer,
                                      IStageService stageService)
        {
            _cacheService = cacheService;
            _localizer = localizer;
            _stageService = stageService;
        }

        public PublicViewModel GetPublicViewModel(int page, string year, string find, Stage? stage, 
                                                  ApplicationUser user)
        {
            page = page <= 0 ? 1 : page;

            var list = _cacheService.GetList(user.Id);

            // Sort by date.
            if (year != null && year != _localizer["All"] && int.TryParse(year, out int result))
            {
                list = list.Where(i => i.Date.Year == result).ToList();
            }

            // Sort by title.
            if (!string.IsNullOrEmpty(find))
            {
                list = list.Where(p => p.Title.ToLower().Contains(find.Trim().ToLower())).ToList();
            }

            // Sort by stage.
            if (stage != null)
            {
                list = list.Where(p => p.Stage == stage).ToList();
            }

            var items = list.Skip((page - 1) * Common.PageSize).Take(Common.PageSize);

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
                Name = user.Name,
                Surname = user.Surname,
                UniqueUrl = user.UniqueUrl,
                MiddleName = user.MiddleName,
                ImageData = user.Photo,
                Controller = "Public",
                Parameters = new Dictionary<string, string>
                {
                    ["uniqueUrl"] = user.UniqueUrl
                },
                PageViewModel = new PageViewModel(list.Count, page, Common.PageSize)
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
