using ApplicationCore.Entities;
using System.Threading;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces;

public interface ICertificateViewModelService
{
    Task<IndexViewModel> GetIndexViewModel(int page, string userId, string year, string find, Stage? stage);

    Task UpdateCertificateAsync(CertificateViewModel cvm, string userId, CancellationToken cancellationToken = default);
    Task CreateCertificateAsync(CertificateViewModel cvm, string userId, CancellationToken cancellationToken = default);
    Task DeleteCertificateAsync(string id, string userId, CancellationToken cancellationToken = default);

    Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int page, string id, string userId,
                                                                   CancellationToken cancellationToken = default);
    Task<CertificateViewModel> GetCertificateByIdAsync(string id, string userId,
                                                       CancellationToken cancellationToken = default);
}
