using ApplicationCore.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Web;
using Xunit;

namespace UnitTests.Infrastructure.Services
{
    public class GetShortenedUrl
    {
        private readonly IUrlShortener _urlShortener;

        public GetShortenedUrl()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Startup>()
                .Build();

            _urlShortener = new UrlShortener(new HttpClient(), configuration);
        }

        [Fact]
        public async Task ShortUrlRequest()
        {
            string expected = await _urlShortener.GetShortenedUrlAsync("https://example.com");

            Assert.NotEqual("Error", expected);
            Assert.NotEqual(0, expected.Length);
        }
    }
}
