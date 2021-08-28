using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using ApplicationCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
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
        private readonly IFilterService _filterService;
        private readonly IPageService _pageService;

        public PublicViewModelService(ICachedPublicViewModelService cacheService,
                                      IStringLocalizer<SharedResource> localizer,
                                      IStageService stageService,
                                      IFilterService filterService,
                                      IPageService pageService)
        {
            _cacheService = cacheService;
            _localizer = localizer;
            _stageService = stageService;
            _filterService = filterService;
            _pageService = pageService;
        }

        public PublicViewModel GetPublicViewModel(int page, string year, string find, Stage? stage,
                                                  ApplicationUser user)
        {
            page = page <= 0 ? 1 : page;

            var list = _cacheService.GetList(user.Id);

            list = _filterService.FilterOut(list, year, find, stage).ToList();

            var count = list.Count;

            var items = _pageService.GetDataToPage(list, count, ref page);

            var certificates = items.Select(i =>
            {
                var certificateViewModel = new CertificateViewModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Date = i.Date,
                    Path = i.Path,
                    UserId = i.UserId
                };
                return certificateViewModel;
            });

            var years = EnumerableHelper.GetYears(_localizer["All"]);

            var stages = _stageService.GetStages(includeAllValue: true);

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
                PageViewModel = new PageViewModel(count, page, Common.PageSize)
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
                Path = certificate.Path,
                UserId = certificate.UserId,
                Page = page
            };
        }
    }
}
