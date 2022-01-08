using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Web;
using Web.Interfaces;
using Web.Services;
using Web.ViewModels;
using Xunit;

namespace UnitTests.Web.Services;

public class CertificateViewModelServiceTests
{
    private readonly Mock<ICertificateRepository> _mockCertificateRepository;
    private readonly Mock<ICachedPublicViewModelService> _mockCachedService;
    private readonly Mock<IStringLocalizer<SharedResource>> _mockLocalizerService;
    private readonly Mock<IStageService> _mockStageService;
    private readonly Mock<IFilterService> _mockFilterService;
    private readonly Mock<IPageService> _mockPageService;
    private readonly string UserId = "10f";

    public CertificateViewModelServiceTests()
    {
        _mockCertificateRepository = new();
        _mockCachedService = new();
        _mockLocalizerService = new();
        _mockStageService = new();
        _mockFilterService = new();
        _mockPageService = new();

        _mockCertificateRepository.Setup(f => f.ListByUserId(UserId))
                                  .Returns(GetCertificates());

        _mockLocalizerService.Setup(f => f["All"]).Returns(new LocalizedString("All", "All"));

        _mockFilterService.Setup(f => f.FilterOut(It.IsAny<IEnumerable<Certificate>>(),
                                                  It.IsAny<string>(),
                                                  It.IsAny<string>(),
                                                  It.IsAny<Stage>())).Returns(GetCertificates());

        _mockPageService.Setup(f => f.GetDataToPage(It.IsAny<IEnumerable<Certificate>>(),
                                                    It.IsAny<int>(),
                                                    ref It.Ref<int>.IsAny,
                                                    true)).Returns(GetCertificates());

        _mockStageService.Setup(f => f.GetStages(true)).Returns(GetStages());
    }


    [Fact]
    public void ReturnFilteredIndexViewModel()
    {
        // Arrange
        var service = new CertificateViewModelService(_mockCertificateRepository.Object,
                                                      _mockLocalizerService.Object,
                                                      _mockCachedService.Object,
                                                      _mockStageService.Object,
                                                      _mockFilterService.Object,
                                                      _mockPageService.Object);

        // Act
        var result = service.GetIndexViewModel(0, UserId, default, default, default(Stage));

        // Assert
        _mockStageService.Verify(f => f.GetStages(true));
        Assert.Equal(2, result.Certificates.Count());
    }


    private List<Certificate> GetCertificates() =>
        new()
        {
            new Certificate(null, "certificate1", null, null, Stage.International, new DateTime(2017, 1, 1)),
            new Certificate(null, "certificate1", null, null, Stage.District, new DateTime(2017, 1, 1))
        };


    private List<StageViewModel> GetStages() =>
        new()
        {
            new StageViewModel { Name = "stage1" },
            new StageViewModel { Name = "stage2" },
        };
}
