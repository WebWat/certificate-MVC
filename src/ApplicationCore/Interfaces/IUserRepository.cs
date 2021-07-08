using ApplicationCore.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> List(Func<ApplicationUser, bool> predicate);
        IEnumerable<ApplicationUser> ListIncludeCertificates(Func<ApplicationUser, bool> predicate);

        Task<ApplicationUser> GetAsync(Expression<Func<ApplicationUser, bool>> predicate, 
                                       CancellationToken cancellationToken = default);

        Task DeleteUserAsync(string id, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    }
}
