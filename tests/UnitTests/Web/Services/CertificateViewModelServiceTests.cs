using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Collections.Generic;
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

            _mockCertificateRepository.Setup(f => f.ListByUserId(UserId))
                                      .Returns(GetCertificates());

            _localizerService.Setup(f => f["All"]).Returns(new LocalizedString("All", "All"));

            _mockStageService.Setup(f => f.GetStages()).Returns(GetStages());
        }


        [Fact]
        public void ReturnFilteredIndexViewModel()
        {
            // Arrange
            var service = new CertificateViewModelService(_mockCertificateRepository.Object,
                                                          _localizerService.Object,
                                                          _mockCachedService.Object,
                                                          _mockStageService.Object);

            // Act
            var result = service.GetIndexViewModel(0, UserId, "2017", "certificate1", Stage.International);

            // Assert
            _mockStageService.Verify(f => f.GetStages());
            Assert.Single(result.Certificates);
            Assert.Equal(1, result.PageViewModel.PageNumber);
        }


        private List<Certificate> GetCertificates() =>
            new()
            {
                new Certificate(null, "certificate1", null, null, Stage.International, new DateTime(2017, 1, 1)),
                new Certificate(null, "certificate1", null, null, Stage.District, new DateTime(2017, 1, 1)),
                new Certificate(null, "certificate1", null, null, Stage.District, DateTime.UtcNow),
                new Certificate(null, "certificate2", null, null, Stage.District, default),
            };


        private List<StageViewModel> GetStages() =>
            new()
            {
                new StageViewModel { Name = "stage1" },
                new StageViewModel { Name = "stage2" },
            };
    }
}
