using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICertificateRepository : IAsyncRepository<Certificate>
    {
        Task<Certificate> GetCertificateIncludeLinksAsync(int id, string userId);
        Task CreateWithRatingUpdateAsync(Certificate entity);
        Task<Certificate> GetByUserIdAsync(int id, string userId);
        IEnumerable<Certificate> ListByUserId(string userId);
    }
}
