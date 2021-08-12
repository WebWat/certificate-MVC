using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EFCoreRepository<T> : IAsyncDisposable, IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;

        public EFCoreRepository(ApplicationContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }


        public IEnumerable<T> List(Func<T, bool> predicate)
        {
           return _context.Set<T>().AsNoTracking().Where(predicate);
        }


        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }


        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().CountAsync(cancellationToken);
        }


        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
