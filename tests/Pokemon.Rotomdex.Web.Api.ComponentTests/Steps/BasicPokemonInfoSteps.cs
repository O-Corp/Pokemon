using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using NUnit.Framework;
using Pokemon.Rotomdex.Web.Api.Adapters;
using Pokemon.Rotomdex.Web.Api.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Pokemon.Rotomdex.Web.Api.ComponentTests.Steps
{
    [Binding]
    public class BasicPokemonInfoSteps
    {
        private string _pokemonName;
        private HttpResponseMessage _httpResponse;
        private Mock<IPokemonApiAdapter> _pokemonApiAdapter;

        [Before]
        public void Setup()
        {
            _pokemonApiAdapter = new Mock<IPokemonApiAdapter>();
        }
        
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

        [Then(@"a request is made to the Pokemon API")]
        public void ThenARequestIsMadeToThePokemonApi()
        {
            _pokemonApiAdapter.Verify(x => x.GetPokemonDetails(_pokemonName), Times.Once);
        }

        [Then(@"the response is")]
        public async Task ThenTheResponseIs(Table table)
        {
            var expected = table.CreateInstance<PokemonDetails>();
            var result = await _httpResponse.Content.ReadAsAsync<PokemonDetails>();
            
            Assert.That(result.Habitat, Is.EqualTo(expected.Habitat));
            Assert.That(result.Name, Is.EqualTo(expected.Name));
            Assert.That(result.DescriptionStandard, Is.EqualTo(expected.DescriptionStandard));
            Assert.That(result.IsLegendary, Is.EqualTo(expected.IsLegendary));
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