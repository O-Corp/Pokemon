using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Exceptions;
using Rotomdex.Integration.Adapters;
using Rotomdex.Web.Api.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Rotomdex.Web.Api.ComponentTests.Steps
{
    [Scope(Feature = "BasicPokemonInfo")]
    [Binding]
    public class BasicPokemonInfoSteps
    {
        private HttpResponseMessage _httpResponse;
        private DataContainer _dataContainer;
        private Mock<IPokemonApiAdapter> _pokemonApiAdapter;
        private PokeApiResponseBuilder _pokeApiResponseBuilder;

        [Before]
        public void Setup()
        {
            _pokeApiResponseBuilder = new PokeApiResponseBuilder();
            _pokemonApiAdapter = new Mock<IPokemonApiAdapter>();
            _dataContainer = new DataContainer
            {
                ApiAdapter = _pokemonApiAdapter.Object
            };
        }

        [Given("that the pokemon (.*) exists")]
        public void GivenThatThePokemonExists(string pokemon)
        {
            var pokeRequest = new PokeRequest { Name = pokemon };
            _pokeApiResponseBuilder = new PokeApiResponseBuilder()
                .WithValidResponse()
                .WithName(pokemon);
            _pokemonApiAdapter
                 .Setup(x => x.GetPokemon(It.Is<PokeRequest>(y => y.Name == pokeRequest.Name)))
                 .ReturnsAsync(_pokeApiResponseBuilder.Build());
        }
        
        [Given(@"the pokemon (.*) does not exist")]
        public void GivenThePokemonDoesNotExist(string pokemon)
        {
            _pokeApiResponseBuilder.WithInvalidPokemon();
        }

        [Given(@"the third party is unavailable")]
        public void GivenTheThirdPartyIsUnavailable()
        {
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(It.IsAny<PokeRequest>()))
                .Throws(new ThirdPartyUnavailableException("test", new Exception()));
        }

        [When(@"the request is sent to get information about (.*)")]
        public async Task WhenTheRequestIsSentToGetInformationAbout(string pokemon)
        {
            using (var client = TestHelper.CreateHttpClient(_dataContainer))
            {
                _httpResponse = await client.GetAsync($"http://localhost/rotomdex/v1/pokemon/{pokemon}");
            }
        }

        [When(@"a request is made to get information about (.*) in language of (.*)")]
        public async Task WhenARequestIsMadeToGetInformationAboutInLanguageOf(string pokemon, string language)
        {
            using (var client = TestHelper.CreateHttpClient(_dataContainer))
            {
                _httpResponse = await client.GetAsync($"http://localhost/rotomdex/v1/pokemon/{pokemon}?lang={language}");
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
            _pokemonApiAdapter.Verify(x => x.GetPokemon(It.IsAny<PokeRequest>()), Times.Once);
        }

        [Then(@"the response is")]
        public async Task ThenTheResponseIs(Table table)
        {
            var expected = table.CreateInstance<PokemonResponse>();
            var result = await _httpResponse.Content.ReadAsAsync<PokemonResponse>();

            Assert.That(result.Habitat, Is.EqualTo(expected.Habitat));
            Assert.That(result.Name, Is.EqualTo(expected.Name));
            Assert.That(result.DescriptionStandard, Is.EqualTo(expected.DescriptionStandard));
            Assert.That(result.IsLegendary, Is.EqualTo(expected.IsLegendary));
        }
    }
}