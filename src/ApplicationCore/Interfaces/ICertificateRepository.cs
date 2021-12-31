using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces;

public interface ICertificateRepository : IAsyncRepository<Certificate>
{
    Task<Certificate> GetCertificateIncludeLinksAsync(int id,
                                                      string userId,
                                                      CancellationToken cancellationToken = default);

    Task<Certificate> GetByUserIdAsync(int id, string userId, CancellationToken cancellationToken = default);

    IEnumerable<Certificate> ListByUserId(string userId);

    Task DeleteCertificatesByUserId(string userId, CancellationToken cancellationToken = default);
}