using ApplicationCore.Helpers;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.ApplicationCore.Helpers
{
    public class ReturnListOfYears
    {
        [Fact]
        public void ReturnYears()
        {
            // Arrange & Act
            var allValue = "All";
            var result = EnumerableHelper.GetYears(allValue);

            // Assert
            Assert.Equal(DateTime.Now.Year - 1998, result.Count);
            Assert.Equal(allValue, result.First());
        }
    }
}
