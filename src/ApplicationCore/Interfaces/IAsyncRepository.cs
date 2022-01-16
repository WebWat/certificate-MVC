using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces;

public interface IAsyncRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> List(Func<T, bool> predicate);

    Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}
