using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Controllers
{
    [Collection("Sequential")]
    public class PublicControllerIndex : IClassFixture<WebTestFixture>
    {
        private readonly string url = "NTE4ZjE2NWMtMTMwOS00MmYwLWFmZTUtMGMyZTBjNmU5NzY5MTkwNzMzYmEtMDE5MS00ZWFkLTlkOWQtZGQ3M2RiMDkxZjMy";

        public PublicControllerIndex(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task ReturnsIndexWithCertificateListing()
        {
            var response = await Client.GetAsync("/Public?code=" + url);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("Test Description", stringResponse);
        }

        [Fact]
        public async Task ReturnsDetailsWithCertificateAndLinks()
        {        
            var response = await Client.GetAsync("/Public?code=" + url);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            string id = GetCertificateId(stringResponse);

            response = await Client.GetAsync($"/Public/Details?id={id}&code=" + url);
            response.EnsureSuccessStatusCode();
            stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("Test Description", stringResponse);
            Assert.Contains("Test Link", stringResponse);
        }

        private string GetCertificateId(string input)
        {
            var regex = new Regex(@"start\('(\d+)");
            var match = regex.Match(input);
            return match.Groups.Values.LastOrDefault().Value;
        }
    }
}
