using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System.Linq;
using System.Threading;
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
        private readonly ICachedPublicViewModelService _cacheService;

        public LinkViewModelService(IAsyncRepository<Link> linkRepository, 
                                    IUrlShortener urlShortener, 
                                    ICertificateRepository certificateRepository, 
                                    ICachedPublicViewModelService cacheService)
        {
            _linkRepository = linkRepository;
            _urlShortener = urlShortener;
            _certificateRepository = certificateRepository;
            _cacheService = cacheService;
        }

        public async Task CreateLinkAsync(int certificateId, LinkViewModel lvm, string userId, CancellationToken cancellationToken = default)
        {
            var links = await _certificateRepository.GetCertificateIncludeLinksAsync(certificateId, userId, cancellationToken);

            // Check the number of links.
            // TODO: rewrite
            if (links.Links.Count >= 5)
            {
                return;
            }

            await _linkRepository.CreateAsync(new Link
            {
                Id = lvm.Id,
                Name = await _urlShortener.GetShortenedUrlAsync(lvm.Name),
                CertificateId = certificateId,
                UserId = userId
            }, cancellationToken);

            await _cacheService.SetItemAsync(certificateId, userId);
        }

        public async Task<LinkListViewModel> GetLinkListViewModelAsync(int certificateId, string userId, CancellationToken cancellationToken = default)
        {
            var links = await _certificateRepository.GetCertificateIncludeLinksAsync(certificateId, userId, cancellationToken);

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

        public async Task<int> DeleteLinkAsync(int id, string userId, CancellationToken cancellationToken = default)
        {
            var link = await _linkRepository.GetAsync(i => i.Id == id && i.UserId == userId, cancellationToken);

            await _linkRepository.DeleteAsync(link, cancellationToken);

            await _cacheService.SetItemAsync(link.CertificateId, userId);

            return link.CertificateId;
        }
    }
}
