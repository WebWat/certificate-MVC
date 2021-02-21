using ApplicationCore.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Web;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Infrastructure.Services
{
    public class GetShortenedUrl
    {
        private readonly IUrlShortener _urlShortener;
        private readonly ITestOutputHelper _output;
        private static ServiceProvider _provider;

        public GetShortenedUrl(ITestOutputHelper output)
        {
            _output = output;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Startup>()
                .Build();

            var services = new ServiceCollection();
            services.AddHttpClient();
            _provider = services.BuildServiceProvider();

            var httpClientFactory = _provider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = _provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<UrlShortener>();

            _urlShortener = new UrlShortener(httpClientFactory, configuration, logger);
        }

        [Fact]
        public async Task ShortUrlRequest()
        {
            //Arrange & Act
            string expected = await _urlShortener.GetShortenedUrlAsync("https://example.com");

            _output.WriteLine("Actual link: " + expected);

            //Assert
            Assert.NotEqual("Error", expected);
        }
    }
}
