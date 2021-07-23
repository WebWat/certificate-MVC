using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Controllers
{
    [Collection("Sequential")]
    public class CertificateControllerIndex : IClassFixture<WebTestFixture>
    {
        public CertificateControllerIndex(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }


        [Fact]
        public async Task RedirectsToLoginIfNotAuthenticated()
        {
            var response = await Client.GetAsync("/certificate");

            Assert.Contains("/Identity/Account/Login", response.RequestMessage.RequestUri.ToString());
        }
    }
}
