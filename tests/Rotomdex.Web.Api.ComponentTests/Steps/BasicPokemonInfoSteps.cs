using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Rotomdex.Domain.Adapters;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Exceptions;
using Rotomdex.Domain.Models;
using Rotomdex.Web.Api.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Rotomdex.Web.Api.ComponentTests.Steps
{
    [Binding]
    public class BasicPokemonInfoSteps
    {
        private PokeRequest _pokeRequest;
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
            _pokeRequest = new PokeRequest { Name = name };
            var pokemon = new Pokemon(name, "It was created by a scientist.", "rare", true);
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(It.Is<PokeRequest>(y => y.Name == name)))
                .ReturnsAsync(pokemon);
        }

        [Given(@"an invalid pokemon")]
        public void GivenAnInvalidPokemon()
        {
            _pokeRequest = new PokeRequest { Name = "xxx" };
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(_pokeRequest))
                .ReturnsAsync((Pokemon)null);
        }

        [Given(@"the third party is unavailable")]
        public void GivenTheThirdPartyIsUnavailable()
        {
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(It.IsAny<PokeRequest>()))
                .ThrowsAsync(new ThirdPartyUnavailableException("testing", new Exception()));
        }

        [When(@"the request is sent")]
        public async Task WhenTheRequestIsSent()
        {
            using (var client = TestHelper.CreateHttpClient(_dataContainer))
            {
                _httpResponse = await client.GetAsync($"http://localhost/rotomdex/v1/pokemon/{_pokeRequest.Name}");
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