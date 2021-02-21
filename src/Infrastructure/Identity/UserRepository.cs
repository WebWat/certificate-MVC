using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task DeleteUserAsync(string id)
        {
            var _user = _context.Users.Include(i => i.Certificates).FirstOrDefault(i => i.Id == id);

            _user.ClearCertificates();

            _context.Users.Remove(_user);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Users.AsNoTracking().CountAsync();
        }

        public async Task<ApplicationUser> GetAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<ApplicationUser> List(Func<ApplicationUser, bool> predicate)
        {
            return _context.Users.AsNoTracking().Where(predicate);
        }

        public IEnumerable<ApplicationUser> ListIncludeCertificates(Func<ApplicationUser, bool> predicate)
        {
            return _context.Users.AsNoTracking().Include(i => i.Certificates).Where(predicate);
        }
    }
}
