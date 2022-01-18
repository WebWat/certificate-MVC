using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        var cer = await GetAsync(i => i.Id == id && i.UserId == userId, cancellationToken);

        cer.Links = (await _context.Links.ToListAsync()).Where(g => g.CertificateId == cer.Id).ToList();

        return cer;
    }


    public async Task DeleteCertificatesByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var listCer = await ListByUserId(userId);

        foreach(var item in listCer)
        {
            _context.Links.RemoveRange((await _context.Links.ToListAsync()).Where(g => g.CertificateId == item.Id).ToList());
        }

        _context.Certificates.RemoveRange(listCer);

        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<IEnumerable<Certificate>> ListByUserId(string userId)
    {
        return await List(i => i.UserId == userId);
    }
}