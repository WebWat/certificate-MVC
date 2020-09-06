using ApplicationCore.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Expression<Func<User, bool>> predicate);
        Task<User> GetUserIncludeCertificatesAsync(Expression<Func<User, bool>> predicate);
        Task DeleteUserAsync(string id);
        IEnumerable<User> List(Func<User, bool> predicate);
        IEnumerable<User> ListIncludeCertificates(Func<User, bool> predicate);
        Task<int> GetCountAsync();
    }
}
