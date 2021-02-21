using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UrlShortener> _logger;

        public UrlShortener(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<UrlShortener> logger)
        {
            _clientFactory = clientFactory;
            _apiKey = configuration["Api:Key"];
            _logger = logger;
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
                _logger.LogInformation("The request was successful");

                string result = await response.Content.ReadAsStringAsync();

                return result.Replace("\"", "");
            }

            _logger.LogWarning("An error occurred while executing the query: " + response.StatusCode);

            //If the status code is not successful
            return "Error";
        }
    }
}
