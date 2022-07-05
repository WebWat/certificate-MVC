using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity;

public class UserRepository : IUserRepository
{
    public readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }


    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.AsNoTracking().CountAsync(cancellationToken);
    }


    public async Task<ApplicationUser?> GetAsync(Func<ApplicationUser, bool> predicate,
                                                CancellationToken cancellationToken = default)
    {
        IEnumerable<ApplicationUser> data = await _context.Users.AsNoTracking().ToListAsync(cancellationToken);

        return data.FirstOrDefault(predicate);
    }


    public IEnumerable<ApplicationUser> List(Func<ApplicationUser, bool> predicate)
    {
        return _context.Users.AsNoTracking().Where(predicate);
    }
}
