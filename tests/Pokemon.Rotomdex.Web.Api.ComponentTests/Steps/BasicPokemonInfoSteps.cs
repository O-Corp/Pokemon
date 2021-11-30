using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Pokemon.Rotomdex.Domain.Adapters;
using Pokemon.Rotomdex.Domain.Models;
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
        private DataContainer _dataContainer;
        private Mock<IPokemonApiAdapter> _pokemonApiAdapter;

        [Before]
        public void Setup()
        {
            _pokemonApiAdapter = new Mock<IPokemonApiAdapter>();
            _dataContainer = new DataContainer
            {
                ApiAdapter = _pokemonApiAdapter.Object
            };
        }
        
        [Given(@"a valid request to retrieve information about (.*)")]
        public void GivenAValidRequestToRetrieveInformationAbout(string name)
        {
            _pokemonName = name;
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(name))
                .ReturnsAsync(new Pokemonster(_pokemonName, "description", "rare", true));
        }

        [When(@"the request is sent")]
        public async Task WhenTheRequestIsSent()
        {
            using (var client = TestHelper.CreateHttpClient(_dataContainer))
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
            _pokemonApiAdapter.Verify(x => x.GetPokemon(_pokemonName), Times.Once);
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
}