using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Helpers;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services;

public class CertificateViewModelService : ICertificateViewModelService
{
    private readonly ICertificateRepository _repository;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ICachedPublicViewModelService _cacheService;
    private readonly IStageService _stageService;
    private readonly IFilterService _filterService;
    private readonly IPageService _pageService;

    public CertificateViewModelService(ICertificateRepository repository,
                                       IStringLocalizer<SharedResource> localizer,
                                       ICachedPublicViewModelService cacheService,
                                       IStageService stageService,
                                       IFilterService filterService,
                                       IPageService pageService)
    {
        _repository = repository;
        _localizer = localizer;
        _cacheService = cacheService;
        _stageService = stageService;
        _filterService = filterService;
        _pageService = pageService;
    }


    public async Task<IndexViewModel> GetIndexViewModel(int page, string userId, string year, string find, Stage? stage)
    {
        page = page <= 0 ? 1 : page;

        var list = await _repository.ListByUserId(userId);

        list = _filterService.FilterOut(list, year, find, stage);

        var count = list.Count();

        var items = _pageService.GetDataToPage(list, count, ref page, includeCheck: true);

        var certificates = items.Select(i =>
        {
            var certificateViewModel = new CertificateViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Date = i.Date,
                Path = i.Path
            };

            return certificateViewModel;
        });

        var years = EnumerableHelper.GetYears(_localizer["All"]);

        var stages = _stageService.GetStages(includeAllValue: true);

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
            cvm.Path,
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
            cvm.Path,
            cvm.Description,
            cvm.Stage,
            cvm.Date
        );

        await _repository.UpdateAsync(certificate, cancellationToken);

        await _cacheService.SetItemAsync(certificate.Id, userId);
        _cacheService.SetList(userId);
    }


    public async Task DeleteCertificateAsync(string id,
                                             string userId,
                                             CancellationToken cancellationToken = default)
    {
        var certificate = await _repository.GetByUserIdAsync(id, userId, cancellationToken);

        await _repository.DeleteAsync(certificate, cancellationToken);

        _cacheService.SetList(userId);
    }


    public async Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int page,
                                                                                string id,
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
            Path = certificate.Path,
            UserId = certificate.UserId,
            Page = page
        };
    }


    public async Task<CertificateViewModel> GetCertificateByIdAsync(string id,
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
            Path = certificate.Path
        };
    }
}
