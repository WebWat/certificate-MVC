using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Services;
using Xunit;

namespace UnitTests.Web.Services
{
    public class CachedPublicViewModelServiceTests
    {
        private readonly Mock<ICertificateRepository> _mockCertificateRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly string UserId = "10f";

        public CachedPublicViewModelServiceTests()
        {
            _mockCertificateRepository = new();

            _mockCertificateRepository.Setup(f => f.ListByUserId(It.IsAny<string>())).Returns(GetCertificates());
            _mockCertificateRepository.Setup(f => f.GetCertificateIncludeLinksAsync(default, default, default))
                                      .ReturnsAsync(GetCertificates().First());

            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }


        [Fact]
        public async Task GetCertificateWithLinksGivenIdAndUserId()
        {
            // Arrange
            var service = new CachedPublicViewModelService(_mockCertificateRepository.Object,
                                                           _memoryCache);

            // Act
            var result = await service.GetItemAsync(0, default);

            // Assert
            Assert.Equal("certificate_one", result.Title);
            Assert.NotNull(_memoryCache.Get("CertificateViewModel_0"));
        }


        [Fact]
        public void GetCertificatesListGivenUserId()
        {
            // Arrange
            var service = new CachedPublicViewModelService(_mockCertificateRepository.Object,
                                                           _memoryCache);

            // Act
            var result = service.GetList(UserId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(_memoryCache.Get("PublicViewModel_" + UserId));
        }


        [Fact]
        public async Task SetCertificateCacheGivenIdAndUserId()
        {
            // Arrange
            var service = new CachedPublicViewModelService(_mockCertificateRepository.Object,
                                                           _memoryCache);

            // Act
            await service.SetItemAsync(0, default);

            // Assert
            _mockCertificateRepository.Verify(f => f.GetCertificateIncludeLinksAsync(default,
                                                                                     default,
                                                                                     default));
            Assert.NotNull(_memoryCache.Get("CertificateViewModel_0"));
        }


        [Fact]
        public void SetCertificatesListCacheGivenUserId()
        {
            // Arrange
            var service = new CachedPublicViewModelService(_mockCertificateRepository.Object,
                                                           _memoryCache);

            // Act
            service.SetList(UserId);

            // Assert
            _mockCertificateRepository.Verify(f => f.ListByUserId(UserId));
            Assert.NotNull(_memoryCache.Get("PublicViewModel_" + UserId));
        }


        private List<Certificate> GetCertificates() =>
            new()
            {
                new Certificate(null, "certificate_one", null, null, Stage.AllRussian, DateTime.Now),
                new Certificate(null, "certificate_two", null, null, Stage.AllRussian, DateTime.Now),
            };
    }
}
