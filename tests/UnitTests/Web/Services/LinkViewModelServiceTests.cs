using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.Services;
using Web.ViewModels;
using Xunit;
namespace UnitTests.Web.Services
{
    public class LinkViewModelServiceTests
    {
        private readonly Mock<ICertificateRepository> _mockCertificateRepository;
        private readonly Mock<ICachedPublicViewModelService> _mockCachedService;
        private readonly Mock<IAsyncRepository<Link>> _mockLinkRepository;
        private readonly Mock<IUrlShortener> _mockUrlShortener;
        private readonly string UserId = "10f";

        public LinkViewModelServiceTests()
        {
            _mockCertificateRepository = new();
            _mockCachedService = new();
            _mockLinkRepository = new();
            _mockUrlShortener = new();

            _mockCertificateRepository.Setup(f => f.GetCertificateIncludeLinksAsync(0, UserId, default))
                                      .ReturnsAsync(GetCertificate());
            _mockLinkRepository.Setup(f => f.GetAsync(It.IsAny<Expression<Func<Link, bool>>>(), default))
                               .ReturnsAsync(GetLink());

            _mockUrlShortener.Setup(f => f.GetShortenedUrlAsync(default)).ReturnsAsync("test");
        }


        [Fact]
        public async Task CreateLink()
        {
            // Arrange
            var service = new LinkViewModelService(_mockLinkRepository.Object,
                                                   _mockUrlShortener.Object,
                                                   _mockCertificateRepository.Object,
                                                   _mockCachedService.Object);

            // Act
            await service.CreateLinkAsync(0, GetLVM(), UserId, default);

            // Assert
            _mockCertificateRepository.Verify(f => f.GetCertificateIncludeLinksAsync(0, UserId, default));
            _mockLinkRepository.Verify(f => f.CreateAsync(It.IsAny<Link>(), default));
            _mockUrlShortener.Verify(f => f.GetShortenedUrlAsync(GetLVM().Name));
            _mockCachedService.Verify(f => f.SetItemAsync(0, UserId));
            Assert.True(true);
        }


        [Fact]
        public async Task DeleteLinkWithCertificateIdReturn()
        {
            // Arrange
            var service = new LinkViewModelService(_mockLinkRepository.Object,
                                                   _mockUrlShortener.Object,
                                                   _mockCertificateRepository.Object,
                                                   _mockCachedService.Object);

            // Act
            var result = await service.DeleteLinkAsync(0, UserId, default);

            // Assert
            _mockLinkRepository.Verify(f => f.GetAsync(It.IsAny<Expression<Func<Link, bool>>>(), default));
            _mockLinkRepository.Verify(f => f.DeleteAsync(It.IsAny<Link>(), default));
            _mockCachedService.Verify(f => f.SetItemAsync(GetLink().CertificateId, UserId));
            Assert.Equal(GetLink().CertificateId, result);
        }


        private Certificate GetCertificate() =>
            new()
            {
                Title = "certificate_one",
                Links = new List<Link>
                {
                    new Link { Name = "link" }
                }
            };


        private Link GetLink() =>
            new()
            {
                Name = "link",
                CertificateId = 12
            };


        private LinkViewModel GetLVM() =>
            new()
            {
                Id = 1,
                Name = "link"
            };
    }
}
