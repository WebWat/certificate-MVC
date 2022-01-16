﻿using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class CertificateRepository : EFCoreRepository<Certificate>, ICertificateRepository
{
    public CertificateRepository(ApplicationContext context) : base(context)
    {
    }


    public async Task<Certificate> GetByUserIdAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        return await GetAsync(i => i.Id == id && i.UserId == userId, cancellationToken);
    }


    public async Task<Certificate> GetCertificateIncludeLinksAsync(string id, string userId, CancellationToken cancellationToken = default)
    { 
        return await _context.Certificates.Include(i => i.Links)
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId,
                                                               cancellationToken);
    }


    public async Task DeleteCertificatesByUserId(string userId, CancellationToken cancellationToken = default)
    {
        _context.Certificates.RemoveRange(await ListByUserId(userId));

        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<IEnumerable<Certificate>> ListByUserId(string userId)
    {
        return await List(i => i.UserId == userId);
    }
}