using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Pokemon.Rotomdex.Web.Api.ComponentTests.Steps
{
    [Binding]
    public class BasicPokemonInfoSteps
    {
        private string _pokemonName;
        private HttpResponseMessage _httpResponse;

        [Given(@"a valid request to retrieve information about (.*)")]
        public void GivenAValidRequestToRetrieveInformationAbout(string name)
        {
            _pokemonName = name;
        }

        [When(@"the request is sent")]
        public async Task WhenTheRequestIsSent()
        {
            using (var client = TestHelper.CreateHttpClient())
            {
                _httpResponse = await client.GetAsync($"http://localhost/rotomdex/v1/pokemon/{_pokemonName}");
            }
        }

        [Then(@"an (.*) response is returned")]
        public void ThenAnResponseIsReturned(HttpStatusCode httpStatusCode)
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(httpStatusCode));
        }
    }
    
    internal static class TestHelper
    {
        public static HttpClient CreateHttpClient()
        {
            var hostBuilder = new WebHostBuilder()
                .UseStartup<Startup>(); // TODO: using real Startup, switch this out later

            var server = new TestServer(hostBuilder);
            return server.CreateClient();
        }
    }
}