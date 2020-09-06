using ApplicationCore.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICertificateRepository : IAsyncRepository<Certificate>
    {
        Task<Certificate> GetCertificateIncludeLinksAsync(Expression<Func<Certificate, bool>> predicate);
    }
}
