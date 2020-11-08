using ApplicationCore.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Web;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Infrastructure.Services
{
    //TODO: fix
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

            var httpClientFactory = _provider.GetService<IHttpClientFactory>();

            _urlShortener = new UrlShortener(httpClientFactory, configuration);
        }

        [Fact]
        public async Task ShortUrlRequest()
        {
            string expected = await _urlShortener.GetShortenedUrlAsync("https://example.com");

            _output.WriteLine(expected);

            Assert.NotEqual("Error", expected);
            Assert.NotEqual(0, expected.Length);
        }
    }
}
