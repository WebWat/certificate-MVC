using ApplicationCore.Interfaces;
using Infrastructure.Data;
using IntegrationTests.Builders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Repositories
{
    public class CertificateRepositoryTests
    {
        private readonly ICertificateRepository repository;
        private readonly ApplicationContext _context;

        public CertificateRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "CertificateTestDB")
                .Options;

            _context = new ApplicationContext(dbOptions);

            repository = new Infrastructure.Repositories.CertificateRepository(_context);

            _context.Certificates.Add(CertificateBuilder.GetDefaultValue());
            _context.SaveChanges();

            var certificate = _context.Certificates.AsNoTracking().FirstOrDefault();
            _context.Links.AddRange(LinkBuilder.GetDefaultValues(certificate.Id));
            _context.SaveChanges();
        }


        [Fact]
        public async Task GetCertificateById()
        {
            // Arrange & Act
            var certificate = await _context.Certificates.AsNoTracking().FirstOrDefaultAsync();
            var repositoryResult = await repository.GetByIdAsync(certificate.Id);
            var contextResult = await _context.Certificates.AsNoTracking().FirstOrDefaultAsync(i => i.Id == certificate.Id);

            // Assert
            Assert.Equal(repositoryResult.Id, contextResult.Id);
        }


        // Use for Coyote
        public async Task GetAndUpdateAtTheSameTime()
        {
            _context.Certificates.Add(CertificateBuilder.GetDefaultValue());
            _context.SaveChanges();

            _context.ChangeTracker.Clear();

            var certificate = await _context.Certificates.AsNoTracking().FirstOrDefaultAsync();

            // This code throws an DbUpdateConcurrencyException,
            // so we add a check (try-catch) to the "Edit" methods of the controllers.
            var deleteResult = repository.DeleteAsync(certificate);
            var updateResult = repository.UpdateAsync(new ApplicationCore.Entities.Certificate { Id = certificate.Id, Title = "New title" });

            await Task.WhenAll(deleteResult, updateResult);
        }
    }
}
