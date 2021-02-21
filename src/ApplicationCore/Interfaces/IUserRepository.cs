using ApplicationCore.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetAsync(Expression<Func<ApplicationUser, bool>> predicate);
        Task DeleteUserAsync(string id);
        IEnumerable<ApplicationUser> List(Func<ApplicationUser, bool> predicate);
        IEnumerable<ApplicationUser> ListIncludeCertificates(Func<ApplicationUser, bool> predicate);
        Task<int> GetCountAsync();
    }
}
