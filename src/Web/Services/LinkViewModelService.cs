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
        private readonly ICachedPublicViewModelService _cacheService;

        public LinkViewModelService(IAsyncRepository<Link> linkRepository,
                                    ICertificateRepository certificateRepository,
                                    ICachedPublicViewModelService cacheService)
        {
            _linkRepository = linkRepository;
            _certificateRepository = certificateRepository;
            _cacheService = cacheService;
        }

        // TODO: change to Status?
        public async Task<bool> CreateLinkAsync(LinkViewModel lvm,
                                                string userId,
                                                CancellationToken cancellationToken = default)
        {
            var certificate = await _certificateRepository.GetCertificateIncludeLinksAsync(lvm.CertificateId,
                                                                                           userId,
                                                                                           cancellationToken);
            if (certificate != null)
            {
                if (certificate.Links.Count >= 5)
                {
                    return true;
                }

                await _linkRepository.CreateAsync(new Link(lvm.Url, lvm.CertificateId),
                                                  cancellationToken);

                await _cacheService.SetItemAsync(lvm.CertificateId, userId);

                return true;
            }

            return false;
        }

        public async Task<LinkListViewModel> GetLinkListViewModelAsync(int certificateId,
                                                                       string userId,
                                                                       CancellationToken cancellationToken = default)
        {
            var certificate = await _certificateRepository.GetCertificateIncludeLinksAsync(certificateId,
                                                                                           userId,
                                                                                           cancellationToken);

            if (certificate is null)
            {
                return null;
            }

            return new LinkListViewModel
            {
                CertificateId = certificateId,
                Links = certificate.Links.Select(e =>
                {
                    var link = new LinkViewModel
                    {
                        Id = e.Id,
                        Url = e.Url,
                        CertificateId = e.CertificateId
                    };

                    return link;
                })
            };
        }

        public async Task<bool> DeleteLinkAsync(int id,
                                                int certificateId,
                                                string userId,
                                                CancellationToken cancellationToken = default)
        {
            var certificate = await _certificateRepository.GetCertificateIncludeLinksAsync(certificateId,
                                                                                           userId,
                                                                                           cancellationToken);

            if (certificate != null)
            {
                var link = certificate.Links.FirstOrDefault(i => i.Id == id);

                if (link != null)
                {
                    await _linkRepository.DeleteAsync(link, cancellationToken);

                    await _cacheService.SetItemAsync(link.CertificateId, userId);

                    return true;
                }
            }

            return false;
        }
    }
}
