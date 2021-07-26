using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web;
using Web.Interfaces;
using Web.Services;
using Web.ViewModels;
using Xunit;

namespace UnitTests.Web.Services
{
    public class CertificateViewModelServiceTests
    {
        private readonly Mock<ICertificateRepository> _mockCertificateRepository;
        private readonly Mock<ICachedPublicViewModelService> _mockCachedService;
        private readonly Mock<IStringLocalizer<SharedResource>> _localizerService;
        private readonly Mock<IStageService> _mockStageService;
        private readonly string UserId = "10f";

        public CertificateViewModelServiceTests()
        {
            _mockCertificateRepository = new();
            _mockCachedService = new();
            _localizerService = new();
            _mockStageService = new();

            _mockCertificateRepository.Setup(f => f.ListByUserId(It.IsAny<string>()))
                                      .Returns(GetCertificates());
            _mockCertificateRepository.Setup(f => f.GetByIdAsync(It.IsAny<int>(), default))
                                      .ReturnsAsync(GetCertificate());

            _localizerService.Setup(f => f["All"]).Returns(new LocalizedString("All",  "All"));

            _mockStageService.Setup(f => f.GetStages()).Returns(GetStages());
        }


        [Fact]
        public void ReturnIndexViewModel()
        {
            // Arrange
            var service = new CertificateViewModelService(_mockCertificateRepository.Object, 
                                                          _localizerService.Object,
                                                          _mockCachedService.Object,
                                                          _mockStageService.Object);

            // Act
            var result = service.GetIndexViewModel(0, default, "2017", "certificate1", Stage.International);

            // Assert
            _mockStageService.Verify(f => f.GetStages());
            Assert.Single(result.Certificates);
            Assert.Equal(1, result.PageViewModel.PageNumber);
        }


        [Fact]
        public async Task CreateCertificate()
        {
            // Arrange
            var service = new CertificateViewModelService(_mockCertificateRepository.Object,
                                                          _localizerService.Object,
                                                          _mockCachedService.Object,
                                                          _mockStageService.Object);

            // Act
            await service.CreateCertificateAsync(GetCVM(), UserId, default);

            // Assert
            _mockCertificateRepository.Verify(f => f.CreateAsync(It.IsAny<Certificate>(), default));
            _mockCachedService.Verify(f => f.SetList(UserId));
            Assert.True(true);
        }


        [Fact]
        public async Task UpdateCertificate()
        {
            // Arrange
            var service = new CertificateViewModelService(_mockCertificateRepository.Object,
                                                          _localizerService.Object,
                                                          _mockCachedService.Object,
                                                          _mockStageService.Object);

            // Act
            await service.UpdateCertificateAsync(GetCVM(), UserId, default);

            // Assert
            _mockCertificateRepository.Verify(f => f.GetByIdAsync(GetCVM().Id, default));
            _mockCertificateRepository.Verify(f => f.UpdateAsync(It.IsAny<Certificate>(), default));
            _mockCachedService.Verify(f => f.SetItemAsync(GetCVM().Id, UserId));
            _mockCachedService.Verify(f => f.SetList(UserId));
            Assert.True(true);
        }


        [Fact]
        public async Task DeleteCertificate()
        {
            // Arrange
            var service = new CertificateViewModelService(_mockCertificateRepository.Object,
                                                          _localizerService.Object,
                                                          _mockCachedService.Object,
                                                          _mockStageService.Object);

            // Act
            await service.DeleteCertificateAsync(0, UserId, default);

            // Assert
            _mockCertificateRepository.Verify(f => f.GetByUserIdAsync(0, UserId, default));
            _mockCertificateRepository.Verify(f => f.DeleteAsync(It.IsAny<Certificate>(), default));
            _mockCachedService.Verify(f => f.SetList(UserId));
            Assert.True(true);
        }


        private List<Certificate> GetCertificates() =>
            new()
            {
                new Certificate { Title = "certificate1", Date = new DateTime(2017, 1, 1), Stage = Stage.International },
                new Certificate { Title = "certificate1", Date = new DateTime(2017, 1, 1), Stage = Stage.District },
                new Certificate { Title = "certificate1", Date = DateTime.UtcNow },
                new Certificate { Title = "certificate2" },
            };


        private Certificate GetCertificate() =>
            new()
            {
                Title = "certificate_two"
            };


        private List<StageViewModel> GetStages() =>
            new()
            {
                new StageViewModel { Name = "stage1" },
                new StageViewModel { Name = "stage2" },
            };


        private CertificateViewModel GetCVM() =>
            new()
            {
                Id = 12,
                Title = default,
                Description = default,
                Date = default,
                Stage = default,
                File = default
            };
    }
}
