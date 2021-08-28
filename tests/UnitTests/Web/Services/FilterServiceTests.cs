using ApplicationCore.Entities;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Web;
using Web.Interfaces;
using Web.Services;
using Xunit;

namespace UnitTests.Web.Services
{
    public class FilterServiceTests
    {
        private readonly Mock<IStringLocalizer<SharedResource>> _mockLocalizerService;
        private readonly IFilterService _filterService;

        public FilterServiceTests()
        {
            _mockLocalizerService = new();

            _mockLocalizerService.Setup(f => f["All"]).Returns(new LocalizedString("All", "All"));

            _filterService = new FilterService(_mockLocalizerService.Object);
        }


        [Fact]
        public void GetFilteredValues()
        {
            // Arrange & Act
            var result = _filterService.FilterOut(GetCertificates(), "2017", "certificate1", Stage.District);

            // Assert
            Assert.Single(result);
        }


        [Fact]
        public void GetFilteredValuesGivenAll()
        {
            // Arrange & Act
            var result = _filterService.FilterOut(GetCertificates(), "All", default, default);

            // Assert
            _mockLocalizerService.Verify(f => f["All"]);
            Assert.Equal(GetCertificates().Count, result.Count());
        }


        [Fact]
        public void RemoveYearGivenInvalidValue()
        {
            // Arrange & Act
            var result = _filterService.FilterOut(GetCertificates(), "notnumber", "certificate1", Stage.District);

            // Assert
            Assert.Equal(2, result.Count());
        }


        private List<Certificate> GetCertificates() =>
          new()
          {
              new Certificate(null, "certificate1", null, null, Stage.International, new DateTime(2017, 1, 1)),
              new Certificate(null, "certificate1", null, null, Stage.District, new DateTime(2017, 1, 1)),
              new Certificate(null, "certificate1", null, null, Stage.District, DateTime.UtcNow),
              new Certificate(null, "certificate2", null, null, Stage.District, default),
          };
    }
}
