using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
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
        private HttpClient Client { get; }

        public UrlShortener(HttpClient client, IConfiguration configuration)
        {
            client.BaseAddress = new Uri("http://url.certfcate.ru");
            client.DefaultRequestHeaders.Add("X-API-KEY", configuration["Api:Key"]);
            Client = client;
        }

        public async Task<string> GetShortenedUrlAsync(string url)
        {
            var response = await Client.PostAsync("", new StringContent(JsonSerializer.Serialize(url), Encoding.UTF8, "application/json"));

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
