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

        public override async Task CreateAsync(Certificate entity)
        {           
            await base.CreateAsync(entity);
            await UpdateUser(entity.UserId, entity.Stage);
        }        

        public async Task<Certificate> GetCertificateIncludeLinksAsync(Expression<Func<Certificate, bool>> predicate)
        {
            return await _context.Certificates.Include(i => i.Links).AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        private async Task UpdateUser(string userId, int rating)
        {
            //When a new certificate is created, add a rating to the current user
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);
            user.Rating += rating;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
