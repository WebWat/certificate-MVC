using ApplicationCore.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> List(Func<ApplicationUser, bool> predicate);

        Task<ApplicationUser> GetAsync(Func<ApplicationUser, bool> predicate,
                                       CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    }
}
