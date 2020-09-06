using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Xunit;

namespace UnitTests.ApplicationCore.Services
{
    public class CreateUniqueUrl
    {
        private readonly IUrlGenerator _urlGenerator;

        public CreateUniqueUrl()
        {
            _urlGenerator = new UrlGenerator();
        }

        [Fact]
        public void GenerateUrl()
        {
            string expected = _urlGenerator.Generate();

            Assert.NotNull(expected);
            Assert.NotEqual(0, expected.Length);
        }
    }
}
