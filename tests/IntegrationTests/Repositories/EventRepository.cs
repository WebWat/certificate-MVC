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
                .UseInMemoryDatabase(databaseName: "Usernewdb")
                .Options;

            _context = new ApplicationContext(dbOptions);

            repository = new EFCoreRepository<Event>(_context);

            _context.Events.AddRange(EventBuilder.GetDefaultValues());
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetEventById()
        {
            var _event = await _context.Events.AsNoTracking().FirstOrDefaultAsync();
            var repositoryResult = await repository.GetByIdAsync(_event.Id);
            var contextResult = await _context.Events.AsNoTracking().FirstOrDefaultAsync(i => i.Id == _event.Id);

            Assert.Equal(repositoryResult.Id, contextResult.Id);
        }
    }
}
