using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Controllers
{
    [Collection("Sequential")]
    public class LinkControllerIndex : IClassFixture<WebTestFixture>
    {
        public LinkControllerIndex(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task RedirectsToLoginIfNotAuthenticated()
        {
            var response = await Client.GetAsync("/Link");

            Assert.Contains("/Identity/Account/Login", response.RequestMessage.RequestUri.ToString());
        }
    }
}
