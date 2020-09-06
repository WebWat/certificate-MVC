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
        private readonly IUrlShortener _urlShortener;
        private readonly ICertificateRepository _certificateRepository;

        public LinkViewModelService(IAsyncRepository<Link> linkRepository, ICertificateRepository certificateRepository, IUrlShortener urlShortener)
        {
            _linkRepository = linkRepository;
            _certificateRepository = certificateRepository;
            _urlShortener = urlShortener;
        }

        public async Task CreateLinkAsync(int certificateId, LinkViewModel cvm, string userId)
        {
            if (await CheckCertificate(certificateId, userId) || await CheckCertificateIncludeLink(certificateId, userId))
            {
                return;
            }

            var certificate = await _certificateRepository.GetCertificateIncludeLinksAsync(i => i.Id == certificateId && i.UserId == userId);

            if (certificate.Links.Count >= 5)
            {
                return;
            }

            await _linkRepository.CreateAsync(new Link
            {
                Id = cvm.Id,
                Name = await _urlShortener.GetShortenedUrlAsync(cvm.Name),
                CertificateId = certificateId
            });
        }

        public async Task<LinkListViewModel> GetCreateLinkListViewModelAsync(int certificateId, string userId)
        {
            if (await CheckCertificate(certificateId, userId))
            {
                return null;
            }

            var links = _linkRepository.List(i => i.CertificateId == certificateId);

            return new LinkListViewModel
            {
                CertificateId = certificateId,
                Links = links.Select(i =>
                {
                    var link = new LinkViewModel
                    {
                        Id = i.Id,
                        CertificateId = i.CertificateId,
                        Name = i.Name
                    };

                    return link;
                })
            };
        }

        public async Task<int> DeleteLinkAsync(int id, string userId)
        {
            var link = await _linkRepository.GetByIdAsync(id);

            if (await CheckCertificate(link.CertificateId, userId))
            {
                return 0;
            }

            await _linkRepository.DeleteAsync(link);

            return link.CertificateId;
        }

        private async Task<bool> CheckCertificate(int certificateId, string userId)
        {
            var certificate = await _certificateRepository.GetAsync(i => i.Id == certificateId && i.UserId == userId);

            if (certificate == null)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> CheckCertificateIncludeLink(int certificateId, string userId)
        {
            var certificate = await _certificateRepository.GetCertificateIncludeLinksAsync(i => i.Id == certificateId && i.UserId == userId);

            if (certificate.Links.Count >= 5)
            {
                return true;
            }

            return false;
        }
    }
}
