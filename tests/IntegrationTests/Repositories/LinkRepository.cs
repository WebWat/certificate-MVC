using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using IntegrationTests.Builders;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Repositories
{
    public class LinkRepository
    {
        private readonly IAsyncRepository<Link> repository;
        private readonly ApplicationContext _context;

        public LinkRepository()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Usernewdb")
                .Options;

            _context = new ApplicationContext(dbOptions);

            repository = new EFCoreRepository<Link>(_context);

            _context.Links.AddRange(LinkBuilder.GetDefaultValues());
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetLinkById()
        {
            var link = await _context.Links.AsNoTracking().FirstOrDefaultAsync();
            var repositoryResult = await repository.GetByIdAsync(link.Id);
            var contextResult = await _context.Links.AsNoTracking().FirstOrDefaultAsync(i => i.Id == link.Id);

            Assert.Equal(repositoryResult.Id, contextResult.Id);
        }
    }
}
