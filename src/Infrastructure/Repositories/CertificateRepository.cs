using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CertificateRepository : EFCoreRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<Certificate> GetByUserIdAsync(int id, string userId)
        {
            return await GetAsync(i => i.Id == id && i.UserId == userId);
        }

        public async Task<Certificate> GetCertificateIncludeLinksAsync(int id, string userId)
        {
            return await _context.Certificates.Include(i => i.Links).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        }

        public IEnumerable<Certificate> ListByUserId(string userId)
        {
            return List(i => i.UserId == userId);
        }
    }
}
