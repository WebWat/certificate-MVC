using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces;

public interface ICertificateRepository : IAsyncRepository<Certificate>
{
    Task<Certificate> GetCertificateIncludeLinksAsync(string id,
                                                      string userId,
                                                      CancellationToken cancellationToken = default);

    Task<Certificate> GetByUserIdAsync(string id, string userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Certificate>> ListByUserId(string userId);

    Task DeleteCertificatesByUserId(string userId, CancellationToken cancellationToken = default);
}