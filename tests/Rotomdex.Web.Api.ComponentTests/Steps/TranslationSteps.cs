using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;
using Rotomdex.Web.Api.ComponentTests.Fakes;
using Rotomdex.Web.Api.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Rotomdex.Web.Api.ComponentTests.Steps
{
    [Binding]
    [Scope(Feature = "Translation")]
    public class TranslationSteps
    {
        private DataContainer _dataContainer;
        private HttpResponseMessage _httpResponse;
        private PokeApiResponseBuilder _pokeApiResponseBuilder;
        private FakeTranslationHttpMessageHandler _yodaHttpMessageHandler;
        private FakeTranslationHttpMessageHandler _shakespeareHttpMessageHandler;
        private Mock<IPokemonApiAdapter> _pokemonApiAdapter;
        private Uri _baseAddress;

        [Before]
        public void Setup()
        {
            _baseAddress = new Uri("http://foo.com");
            _pokeApiResponseBuilder = new PokeApiResponseBuilder().WithValidResponse();
            _pokemonApiAdapter = new Mock<IPokemonApiAdapter>();
            _yodaHttpMessageHandler = new FakeTranslationHttpMessageHandler(File.ReadAllText(@"Data\yoda_translation_response.json"));
            _shakespeareHttpMessageHandler = new FakeTranslationHttpMessageHandler(File.ReadAllText(@"Data\shakespeare_translation_response.json"));
            _dataContainer = new DataContainer
            {
                ApiAdapter = _pokemonApiAdapter.Object,
                YodaTranslationsAdapter = new YodaTranslatorAdapter(new HttpClient(_yodaHttpMessageHandler), _baseAddress),
                ShakespeareTranslationsAdapter = new ShakespeareTranslatorAdapter(new HttpClient(_shakespeareHttpMessageHandler), _baseAddress)
            };
        }

        [Given(@"the pokemon (.*) exists")]
        public void GivenThePokemonExists(string name)
        {
            _pokeApiResponseBuilder.WithName(name);
        }

        [Given(@"its habitat is (.*)")]
        public void GivenItsHabitatIs(string habitat)
        {
            _pokeApiResponseBuilder.WithHabitat(habitat);
        }

        [Given(@"its legendary status is (.*)")]
        public void GivenTheLegendaryStatusIs(bool isLegendary)
        {
            _pokeApiResponseBuilder.WithLegendary(isLegendary);
        }

        [When(@"the POST request is sent")]
        public async Task WhenThePostRequestIsSent()
        {
            var pokeApiResponse = _pokeApiResponseBuilder.Build();
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(It.Is<PokeRequest>(y => y.Name == pokeApiResponse.Name)))
                .ReturnsAsync(pokeApiResponse);

            using (var client = TestHelper.CreateHttpClient(_dataContainer))
            {
                var payload = new TranslationRequest { Name = pokeApiResponse.Name };
                _httpResponse = await client.PostAsJsonAsync($"http://localhost/rotomdex/v1/pokemon/translate", payload);
            }
        }

        [Then(@"an (.*) response is returned")]
        public void ThenAnResponseIsReturned(HttpStatusCode httpStatusCode)
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Then(@"the POST response is")]
        public async Task ThenThePostResponseIs(Table table)
        {
            var expected = table.CreateInstance<PokemonResponse>();
            var result = await _httpResponse.Content.ReadAsAsync<PokemonResponse>();

            Assert.That(result.Habitat, Is.EqualTo(expected.Habitat));
            Assert.That(result.Name, Is.EqualTo(expected.Name));
            Assert.That(result.DescriptionStandard, Is.EqualTo(expected.DescriptionStandard));
            Assert.That(result.IsLegendary, Is.EqualTo(expected.IsLegendary));
        }

        [Then(@"the Yoda translation API is called")]
        public void ThenTheYodaTranslationApiIsCalled()
        {
            Assert.That(_yodaHttpMessageHandler.HttpRequest.RequestUri.ToString(), Is.EqualTo($"{_baseAddress}translate/yoda"));
        }

        [Then(@"the Shakespeare translation API is called")]
        public void ThenTheShakespeareTranslationApiIsCalled()
        {
            Assert.That(_shakespeareHttpMessageHandler.HttpRequest.RequestUri.ToString(), Is.EqualTo($"{_baseAddress}translate/shakespeare"));
        }
    }
}