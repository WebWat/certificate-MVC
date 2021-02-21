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
    public class EventRepository
    {
        private readonly IAsyncRepository<Event> repository;
        private readonly ApplicationContext _context;

        public EventRepository()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "CertificateDB")
                .Options;

            _context = new ApplicationContext(dbOptions);

            repository = new EFCoreRepository<Event>(_context);

            _context.Events.AddRange(EventBuilder.GetDefaultValues());
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetEventById()
        {
            // Arrange & Act
            var repositoryResult = await repository.GetByIdAsync(1);
            var contextResult = await _context.Events.AsNoTracking().FirstOrDefaultAsync(i => i.Id == 1);

            // Assert
            Assert.Equal(repositoryResult.Id, contextResult.Id);
        }
    }
}
