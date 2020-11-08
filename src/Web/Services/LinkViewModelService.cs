using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class LinkViewModelService : ILinkViewModelService
    {
        private readonly IAsyncRepository<Link> _linkRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IUrlShortener _urlShortener;
        private readonly IPublicUpdatingCacheService _cacheService;

        public LinkViewModelService(IAsyncRepository<Link> linkRepository, IUrlShortener urlShortener, ICertificateRepository certificateRepository, IPublicUpdatingCacheService cacheService)
        {
            _linkRepository = linkRepository;
            _urlShortener = urlShortener;
            _certificateRepository = certificateRepository;
            _cacheService = cacheService;
        }

        public async Task CreateLinkAsync(int certificateId, LinkViewModel cvm, string userId)
        {
            var links = await _certificateRepository.GetCertificateIncludeLinksAsync(i => i.Id == certificateId && i.UserId == userId);

            //check the number of links
            if (links.Links.Count() >= 5)
            {
                return;
            }

            await _linkRepository.CreateAsync(new Link
            {
                Id = cvm.Id,
                Name = await _urlShortener.GetShortenedUrlAsync(cvm.Name),
                CertificateId = certificateId,
                UserId = userId
            });

            await _cacheService.SetItemAsync(certificateId, userId);
        }

        public async Task<LinkListViewModel> GetLinkListViewModelAsync(int certificateId, string userId)
        {
            var links = await _certificateRepository.GetCertificateIncludeLinksAsync(i => i.Id == certificateId && i.UserId == userId);

            return new LinkListViewModel
            {
                CertificateId = certificateId,
                Links = links.Links.Select(i =>
                {
                    var link = new LinkViewModel
                    {
                        Id = i.Id,
                        Name = i.Name
                    };

                    return link;
                })
            };
        }

        public async Task<int> DeleteLinkAsync(int id, string userId)
        {
            var link = await _linkRepository.GetAsync(i => i.Id == id && i.UserId == userId);

            await _linkRepository.DeleteAsync(link);

            await _cacheService.SetItemAsync(link.CertificateId, userId);

            return link.CertificateId;
        }
    }
}
