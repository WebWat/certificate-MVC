using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.ApplicationCore.Services
{
    public class CreateUniqueUrl
    {
        private readonly IUrlGenerator _urlGenerator;
        private readonly ITestOutputHelper _output;

        public CreateUniqueUrl(ITestOutputHelper output)
        {
            _urlGenerator = new UrlGenerator();
            _output = output;
        }


        [Fact]
        public void GenerateUrl()
        {
            // Arrange & Act
            string result = _urlGenerator.Generate();

            _output.WriteLine("Generated string: " + result);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(string.Empty, result);
        }
    }
}
