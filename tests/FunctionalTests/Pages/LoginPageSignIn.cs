using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApplicationCore.Constants;
using Xunit;

namespace FunctionalTests.Pages
{
    [Collection("Sequential")]
    public class LoginPageSignIn : IClassFixture<WebTestFixture>
    {
        public LoginPageSignIn(WebTestFixture factory)
        {
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        public HttpClient Client { get; }


        [Fact]
        public async Task ReturnsSuccessfulSignInOnPostWithRedirect()
        {
            var getResponse = await Client.GetAsync("/identity/account/login?returnUrl=%2FCertificate");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse1 = await getResponse.Content.ReadAsStringAsync();
            string token = GetRequestVerificationToken(stringResponse1);

            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("UserNameOrEmail", AuthorizationConstants.UserName),
                new KeyValuePair<string, string>("Password", AuthorizationConstants.Password),
                new KeyValuePair<string, string>("__RequestVerificationToken", token)
            };
            var formContent = new FormUrlEncodedContent(keyValues);

            var postResponse = await Client.PostAsync("/identity/account/login?returnUrl=%2FCertificate", formContent);
            Assert.Equal(HttpStatusCode.Redirect, postResponse.StatusCode);
            Assert.Equal(new Uri("/Certificate", UriKind.Relative), postResponse.Headers.Location);
        }


        private string GetRequestVerificationToken(string input)
        {
            string regexpression = @"name=""__RequestVerificationToken"" type=""hidden"" value=""([-A-Za-z0-9+=/\\_]+?)""";
            var regex = new Regex(regexpression);
            var match = regex.Match(input);
            return match.Groups.Values.LastOrDefault().Value;
        }
    }
}
