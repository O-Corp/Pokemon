using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Contracts.PokeApi;
using Rotomdex.Testing.Common.Fakes;
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
        private FakePokeApiHttpHandler _fakePokeApiHttpHandler;
        private Uri _baseAddress;
        private PokemonApiResponse _pokeApiResponse;

        [Before]
        public void Setup()
        {
            _fakePokeApiHttpHandler = new FakePokeApiHttpHandler();
            _baseAddress = new Uri("https://stub.com/");
            _dataContainer = new DataContainer()
            {
                ApiAdapter = new PokeApiAdapter(
                    new HttpClient(_fakePokeApiHttpHandler)
                    {
                        BaseAddress = _baseAddress
                    })
            };
        }

        [Given("that the pokemon (.*) exists")]
        public void GivenThatThePokemonExists(string pokemon)
        {
            var pokeRequest = new PokeRequest { Name = pokemon };
            var pokeApiResponseBuilder = new PokeApiResponseBuilder()
                .WithValidResponse()
                .WithName(pokemon);
            _pokeApiResponse = pokeApiResponseBuilder.Build();
            
            _fakePokeApiHttpHandler.SetupPokemonResponse(pokeRequest, _pokeApiResponse);
            _fakePokeApiHttpHandler.SetupSpeciesResponse(_pokeApiResponse);
        }
        
        [Given(@"the pokemon (.*) does not exist")]
        public void GivenThePokemonDoesNotExist(string pokemon)
        {
            _fakePokeApiHttpHandler.SetupPokemonResponse(new PokeRequest { Name = pokemon }, null);
        }

        [Given(@"the third party is unavailable")]
        public void GivenTheThirdPartyIsUnavailable()
        {
            _dataContainer.ApiAdapter = new PokeApiAdapter(
                new HttpClient(
                    new ErrorHttpMessageHandler()));
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
            using var client = TestHelper.CreateHttpClient(_dataContainer);
            _httpResponse = await client.GetAsync($"http://localhost/rotomdex/v1/pokemon/{pokemon}?lang={language}");
        }

        [Then(@"an (.*) response is returned")]
        public void ThenAnResponseIsReturned(HttpStatusCode httpStatusCode)
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Then(@"a request is made to the Pokemon API")]
        public void ThenARequestIsMadeToThePokemonApi()
        {
            var pokeInfoExpectedUri = $"{_baseAddress}api/v2/pokemon/{_pokeApiResponse.Name}";
            var pokeSpeciesExpectedUri = $"{_baseAddress}api/v2/pokemon-species/{_pokeApiResponse.Id}";
            
            Assert.That(_fakePokeApiHttpHandler.HttpRequests[0].RequestUri.ToString(), Is.EqualTo(pokeInfoExpectedUri));
            Assert.That(_fakePokeApiHttpHandler.HttpRequests[1].RequestUri.ToString(), Is.EqualTo(pokeSpeciesExpectedUri));
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