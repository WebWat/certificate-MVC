using ApplicationCore.Constants;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Interfaces;
using Web.Services;
using Xunit;

namespace UnitTests.Web.Services;

public class PageServiceTests
{
    private readonly IPageService _pageService;

    public PageServiceTests()
    {
        _pageService = new PageService();
    }


    [Fact]
    public void ReturnDataGivenNegativePage()
    {
        // Arrange & Act
        int page = -10;
        var result = _pageService.GetDataToPage(GetCertificates(), GetCertificates().Count, ref page);

        // Assert
        Assert.Equal(GetCertificates().Count, result.Count());
        Assert.Equal(1, page);
    }


    [Fact]
    public void ReducePageHigherNumberOfSkippedElements()
    {
        // Arrange & Act
        int page = 2;
        var result = _pageService.GetDataToPage(GetCertificatesToPage(),
                                                GetCertificatesToPage().Count,
                                                ref page,
                                                true);

        // Assert
        Assert.Equal(GetCertificatesToPage().Count, result.Count());
        Assert.Equal(1, page);
    }


    private List<Certificate> GetCertificatesToPage()
    {
        var list = new List<Certificate>();

        for (int i = 0; i < Common.PageSize; i++)
        {
            list.Add(new Certificate(null, null, null, null, default, default));
        }

        return list;
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
