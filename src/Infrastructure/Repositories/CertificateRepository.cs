using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CertificateRepository : EFCoreRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<Certificate> GetCertificateIncludeLinksAsync(Expression<Func<Certificate, bool>> predicate)
        {
            return await _context.Certificates.Include(i => i.Links).AsNoTracking().FirstOrDefaultAsync(predicate);
        }
    }
}
