using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using IntegrationTests.Builders;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Repositories;

public class LinkRepositoryTests
{
    private readonly IAsyncRepository<Link> repository;
    private readonly ApplicationContext _context;

    public LinkRepositoryTests()
    {
        var dbOptions = new DbContextOptionsBuilder<ApplicationContext>()
                            .UseInMemoryDatabase(databaseName: "LinkTestDB")
                            .Options;

        _context = new ApplicationContext(dbOptions);

        repository = new EFCoreRepository<Link>(_context);

        _context.Links.AddRange(LinkBuilder.GetDefaultValues());
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetLinkById()
    {
        // Arrange
        var contextResult = await _context.Links.AsNoTracking().FirstOrDefaultAsync();

        // Act
        var repositoryResult = await repository.GetByIdAsync(contextResult.Id);

        // Assert
        Assert.Equal(contextResult.Id, repositoryResult.Id);
    }
}
