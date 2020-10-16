using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Reduces user links
    /// </summary>
    public class UrlShortener : IUrlShortener
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiKey;

        public UrlShortener(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _apiKey = configuration["Api:Key"];
        }

        public async Task<string> GetShortenedUrlAsync(string url)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);

            var dataJson = new StringContent(JsonSerializer.Serialize(url),
                                             Encoding.UTF8,
                                             "application/json");

            var response = await client.PostAsync("http://url.certfcate.ru", dataJson);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                return result.Replace("\"", "");
            }

            //If the status code is not successful
            return "Error";
        }
    }
}
