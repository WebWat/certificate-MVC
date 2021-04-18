using ApplicationCore.Interfaces;
using Infrastructure.Data;
using IntegrationTests.Builders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Repositories
{
    public class CertificateRepository
    {
        private readonly ICertificateRepository repository;
        private readonly ApplicationContext _context;

        public CertificateRepository()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "CertificateDB")
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

        //Use for Coyote
        public async Task GetAndDeleteAtTheSameTime()
        {
            _context.Certificates.Add(CertificateBuilder.GetDefaultValue());
            _context.SaveChanges();

            _context.ChangeTracker.Clear();

            var certificate = await _context.Certificates.AsNoTracking().FirstOrDefaultAsync();

            var deleteResult = repository.DeleteAsync(certificate);
            var getResult = repository.GetByIdAsync(certificate.Id);

            await Task.WhenAll(deleteResult, getResult);
        }
    }
}
