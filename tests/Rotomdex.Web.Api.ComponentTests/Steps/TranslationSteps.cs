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
        private Mock<IPokemonApiAdapter> _pokemonApiAdapter;

        [Before]
        public void Setup()
        {
            _pokeApiResponseBuilder = new PokeApiResponseBuilder().WithValidResponse();
            _pokemonApiAdapter = new Mock<IPokemonApiAdapter>();
            _dataContainer = new DataContainer
            {
                ApiAdapter = _pokemonApiAdapter.Object,
                YodaTranslationsAdapter = new FakeYodaTranslator(),
                ShakespeareTranslationsAdapter = new FakeShakespeareTranslator()
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
            Assert.That(((FakeYodaTranslator)_dataContainer.YodaTranslationsAdapter).Text, Is.Not.Null);
        }

        [Then(@"the Shakespeare translation API is called")]
        public void ThenTheShakespeareTranslationApiIsCalled()
        {
            Assert.That(((FakeShakespeareTranslator)_dataContainer.ShakespeareTranslationsAdapter).Text, Is.Not.Null);
        }
    }
}