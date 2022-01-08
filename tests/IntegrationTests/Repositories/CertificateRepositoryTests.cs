using ApplicationCore.Interfaces;
using Infrastructure.Data;
using IntegrationTests.Builders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Repositories;

public class CertificateRepositoryTests
{
    private readonly ICertificateRepository _repository;
    private readonly ApplicationContext _context;

    public CertificateRepositoryTests()
    {
        var dbOptions = new DbContextOptionsBuilder<ApplicationContext>()
                        .UseInMemoryDatabase(databaseName: "CertificateTestDB")
                        .Options;

        _context = new ApplicationContext(dbOptions);

        _repository = new Infrastructure.Repositories.CertificateRepository(_context);

        _context.Certificates.AddRange(CertificateBuilder.GetDefaultValues());
        _context.SaveChanges();

        var certificate = _context.Certificates.AsNoTracking().FirstOrDefault();
        _context.Links.AddRange(LinkBuilder.GetDefaultValues(certificate.Id));
        _context.SaveChanges();
    }


    [Fact]
    public async Task GetCertificateWithLinks()
    {
        // Arrange
        var certificate = await _context.Certificates.Include(e => e.Links).AsNoTracking().FirstAsync();

        // Act
        var repositoryResult = await _repository.GetCertificateIncludeLinksAsync(certificate.Id,
                                                                                 certificate.UserId);

        // Assert
        Assert.Equal(certificate.Title, repositoryResult.Title);
        Assert.Equal(certificate.Links.Count, repositoryResult.Links.Count);
    }


    [Fact]
    public async Task GetCertificateByUserId()
    {
        // Arrange
        var certificate = await _context.Certificates.AsNoTracking().FirstAsync();

        // Act
        var repositoryResult = await _repository.GetByUserIdAsync(certificate.Id,
                                                                  certificate.UserId);

        // Assert
        Assert.Equal(certificate.Title, repositoryResult.Title);
    }


    [Fact]
    public async Task GetListByUserId()
    {
        // Arrange
        var certificate = await _context.Certificates.AsNoTracking().FirstAsync();
        var list = _context.Certificates.Where(e => e.UserId == certificate.UserId);

        // Act
        var repositoryResult = _repository.ListByUserId(certificate.UserId);

        // Assert
        Assert.Equal(list.Count(), repositoryResult.Count());
    }


    // Use for Coyote.
    /*********************************/
    public async Task GetAndUpdateAtTheSameTime()
    {
        _context.Certificates.AddRange(CertificateBuilder.GetDefaultValues());
        _context.SaveChanges();

        _context.ChangeTracker.Clear();

        var certificate = await _context.Certificates.AsNoTracking().FirstOrDefaultAsync();

        // This code throws an DbUpdateConcurrencyException,
        // so we add a check (try-catch) to the "Edit" methods of the controllers.
        var deleteResult = _repository.DeleteAsync(certificate);
        var updateResult = _repository.UpdateAsync(certificate);

        await Task.WhenAll(deleteResult, updateResult);
    }
    /*********************************/
}