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
                .UseInMemoryDatabase(databaseName: "Usernewdb")
                .Options;

            _context = new ApplicationContext(dbOptions);

            repository = new Infrastructure.Repositories.CertificateRepository(_context);

            _context.Certificates.Add(CertificateBuilder.GetDefaultValue());
            _context.Links.AddRange(LinkBuilder.GetDefaultValues());
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCertificateById()
        {
            var certificate = await _context.Certificates.AsNoTracking().FirstOrDefaultAsync();
            var repositoryResult = await repository.GetByIdAsync(certificate.Id);
            var contextResult = await _context.Certificates.AsNoTracking().FirstOrDefaultAsync(i => i.Id == certificate.Id);

            Assert.Equal(repositoryResult.Id, contextResult.Id);
        }

        [Fact]
        public async Task GetCertificateIncludeLinks()
        {
            var repositoryResult = await repository.GetCertificateIncludeLinksAsync(i => i.Title == "Test");
            var contextResult = await _context.Certificates.Include(i => i.Links).AsNoTracking().FirstOrDefaultAsync(i => i.Title == "Test");

            Assert.NotNull(repositoryResult.Links);
            Assert.NotNull(contextResult.Links);
            Assert.Equal(repositoryResult.Id, contextResult.Id);
            Assert.Equal(repositoryResult.Links.Count, contextResult.Links.Count);
            Assert.Equal(repositoryResult.Links.Count(x => x.CertificateId == repositoryResult.Id), contextResult.Links.Count(x => x.CertificateId == contextResult.Id));
        }
    }
}
