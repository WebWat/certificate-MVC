﻿using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Controllers
{
    [Collection("Sequential")]
    public class PublicControllerIndex : IClassFixture<WebTestFixture>
    {
        private readonly string url = "NTE4ZjE2NWMtMTMwOS00MmYwLWFmZTUtMGMyZTB";

        public PublicControllerIndex(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task ReturnsIndexWithCertificateListing()
        {
            var response = await Client.GetAsync("/Public/" + url);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("Test Description", stringResponse);
        }

        [Fact]
        public async Task ReturnsDetailsWithCertificateAndLinks()
        {
            var response = await Client.GetAsync("/Public/" + url);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            string id = GetCertificateId(stringResponse);

            response = await Client.GetAsync($"/Public/Details/{url}/{id}");
            response.EnsureSuccessStatusCode();
            stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("Test Description", stringResponse);
            Assert.Contains("http://url.certfcate.ru", stringResponse);
        }

        private string GetCertificateId(string input)
        {
            var regex = new Regex(@"start\('(\d+)");
            var match = regex.Match(input);
            return match.Groups.Values.LastOrDefault().Value;
        }
    }
}
