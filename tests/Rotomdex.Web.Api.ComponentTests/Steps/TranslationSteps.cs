﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Contracts.PokeApi;
using Rotomdex.Testing.Common.Fakes;
using Rotomdex.Testing.Common.Helpers;
using Rotomdex.Web.Api.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Rotomdex.Web.Api.ComponentTests.Steps
{
    [Binding]
    [Scope(Feature = "Translation")]
    public class TranslationSteps
    {
        private readonly Uri _baseAddress = new("https://funtranslations.com/");
        private DataContainer _dataContainer;
        private HttpResponseMessage _httpResponse;
        private PokemonApiResponse _pokeApiResponse;
        private Mock<IPokemonApiAdapter> _pokemonApiAdapter;
        private HttpClientBuilder _httpClientBuilder;

        [Before]
        public void Setup()
        {
            _httpClientBuilder = new HttpClientBuilder(_baseAddress);
            _pokemonApiAdapter = new Mock<IPokemonApiAdapter>();
            _dataContainer = new DataContainer
            {
                ApiAdapter = _pokemonApiAdapter.Object,
                YodaTranslationsAdapter = new YodaTranslatorAdapter(_httpClientBuilder.WithYodaTranslation().Build()),
                ShakespeareTranslationsAdapter = new ShakespeareTranslatorAdapter(_httpClientBuilder.WithShakespeareTranslation().Build())
            };
        }

        [Given("the pokemon exists")]
        public void GivenThePokemonExists(Table table)
        {
            var pokemon = Pokemon.Create(
                table.Rows[0]["Name"],
                table.Rows[0]["Description"],
                table.Rows[0]["Habitat"],
                bool.Parse(table.Rows[0]["Is Legendary"]));
            
            _pokeApiResponse = new PokeApiResponseBuilder()
                .WithValidResponse()
                .WithName(pokemon.Name)
                .WithHabitat(pokemon.Habitat)
                .WithLegendary(pokemon.IsLegendary)
                .WithDescription(pokemon.Description)
                .Build();
        }

        [Given(@"the translation API is unavailable")]
        public void GivenTheTranslationApiIsUnavailable()
        {
            var httpClient = _httpClientBuilder.WithUnavailableApi().Build();
            _dataContainer.ShakespeareTranslationsAdapter = new ShakespeareTranslatorAdapter(httpClient);
            _dataContainer.YodaTranslationsAdapter = new YodaTranslatorAdapter(httpClient);
        }
        
        [When(@"the POST request is sent")]
        public async Task WhenThePostRequestIsSent()
        {
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(It.Is<PokeRequest>(y => y.Name == _pokeApiResponse.Name)))
                .ReturnsAsync(_pokeApiResponse);

            _pokemonApiAdapter
                .Setup(x => x.GetSpeciesDetails(It.Is<PokeRequest>(y => y.Id == _pokeApiResponse.Id.ToString())))
                .ReturnsAsync(_pokeApiResponse.SpeciesDetails);
            
            using var client = TestHelper.CreateHttpClient(_dataContainer);
            var payload = new TranslationRequest { Name = _pokeApiResponse.Name };
            _httpResponse = await client.PostAsJsonAsync($"http://localhost/rotomdex/v1/pokemon/translate", payload);
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

        [Then(@"the (.*) translation API is called")]
        public void ThenTheTranslationApiIsCalled(string translation)
        {
            switch (translation.ToLower())
            {
                case "yoda":
                    ThenTheYodaTranslationApiIsCalled();
                    break;
                case "shakespeare":
                    ThenTheShakespeareTranslationApiIsCalled();
                    break;
                default:
                    Assert.Fail("No matching translation, please ensure it is supported.");
                    break;
            }
        }
        
        private void ThenTheYodaTranslationApiIsCalled()
        {
            var httpRequestMessage = FakeYodaTranslationHttpMessageHandler.ExecutedRequest;
            if (httpRequestMessage?.RequestUri != null)
            {
                Assert.That(httpRequestMessage.RequestUri.ToString(), Is.EqualTo($"{_baseAddress}translate/yoda"));
                return;
            }
            
            Assert.Fail("Yoda API Translation was not called.");
        }
        
        private void ThenTheShakespeareTranslationApiIsCalled()
        {
            var httpRequestMessage = FakeShakespeareTranslationHttpMessageHandler.ExecutedRequest;
            if (httpRequestMessage?.RequestUri != null)
            {
                Assert.That(httpRequestMessage.RequestUri.ToString(), Is.EqualTo($"{_baseAddress}translate/shakespeare"));
                return;
            }
            
            Assert.Fail("Shakespeare API Translation was not called.");        }
    }
}