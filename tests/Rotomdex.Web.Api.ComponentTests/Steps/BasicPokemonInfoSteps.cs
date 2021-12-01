using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Exceptions;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Contracts;
using Rotomdex.Web.Api.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Rotomdex.Web.Api.ComponentTests.Steps
{
    [Binding]
    public class BasicPokemonInfoSteps
    {
        private const string EnglishDescription = "It was created by a scientist.";
        private const string FrenchDescription = "Il a été créé par un scientifique.";
        private const string EnglishLanguageCode = "en";
        private const string FrenchLanguageCode = "fr";
        
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
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(It.Is<PokeRequest>(y => y.Name == name)))
                .ReturnsAsync(new PokeApiResponse
                {
                    Id = 123,
                    Name = name,
                    Species = new Species { Url = new Uri("http://foo.com/species/123") },
                    SpeciesDetails = new SpeciesDetails
                    {
                        Habitat = new Habitat { Name = "rare" },
                        IsLegendary = true,
                        FlavorTextEntries = new List<Description>
                        {
                            new()
                            {
                                FlavourText = EnglishDescription,
                                Language = new Language { Name = EnglishLanguageCode }
                            },
                            new()
                            {
                                FlavourText = FrenchDescription,
                                Language = new Language { Name = FrenchLanguageCode }
                            }
                        },
                    }
                });
        }

        [Given(@"with language of (.*)")]
        public void GivenWithLanguageOf(string language)
        {
            _pokeRequest.Language = language;
        }

        [Given(@"an invalid pokemon")]
        public void GivenAnInvalidPokemon()
        {
            _pokeRequest = new PokeRequest { Name = "xxx" };
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(_pokeRequest))
                .ReturnsAsync((PokeApiResponse)null);
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
                _httpResponse = await client.GetAsync($"http://localhost/rotomdex/v1/pokemon/{_pokeRequest.Name}?lang={_pokeRequest.Language}");
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