using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Contracts;
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
        private Mock<IPokemonApiAdapter> _pokemonApiAdapter;
        private string _habitat;
        private bool _isLegendary;
        private string _pokemon;

        [Before]
        public void Setup()
        {
            _pokemonApiAdapter = new Mock<IPokemonApiAdapter>();
            _dataContainer = new DataContainer
            {
                ApiAdapter = _pokemonApiAdapter.Object
            };
        }
        
        [Given(@"the pokemon (.*) exists")]
        public void GivenThePokemonExists(string name)
        {
            _pokemon = name;
        }
        
        [Given(@"its habitat is (.*)")]
        public void GivenItsHabitatIs(string habitat)
        {
            _habitat = habitat;
        }
        
        [Given(@"its legendary status is (.*)")]
        public void GivenTheLegendaryStatusIs(bool isLegendary)
        {
            _isLegendary = isLegendary;
        }
        
        [When(@"the POST request is sent")]
        public async Task WhenThePostRequestIsSent()
        {
            _pokemonApiAdapter
                .Setup(x => x.GetPokemon(It.Is<PokeRequest>(y => y.Name == _pokemon)))
                .ReturnsAsync(new PokeApiResponse
                {
                    Id = 123,
                    Name = _pokemon,
                    Species = new Species { Url = new Uri("http://foo.com/species/123") },
                    SpeciesDetails = new SpeciesDetails
                    {
                        Habitat = new Habitat { Name = _habitat },
                        IsLegendary = _isLegendary,
                        FlavorTextEntries = new List<Description>
                        {
                            new()
                            {
                                FlavourText = "It was created by a scientist.",
                                Language = new Language { Name = "en" }
                            }
                        }
                    }
                });
            
            using (var client = TestHelper.CreateHttpClient(_dataContainer))
            {
                var payload = new TranslationRequest { Name = _pokemon };
                _httpResponse = await client.PostAsJsonAsync($"http://localhost/rotomdex/v1/pokemon/translate", payload);
            }
        }

        [Then(@"an (.*) response is returned")]
        public void ThenAnResponseIsReturned(HttpStatusCode httpStatusCode)
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Then(@"the response is translated with")]
        public async Task ThenTheResponseIsTranslatedWith(Table table)
        {
            var expected = table.CreateInstance<PokemonResponse>();
            var result = await _httpResponse.Content.ReadAsAsync<PokemonResponse>();

            Assert.That(result.Habitat, Is.EqualTo(expected.Habitat));
            Assert.That(result.Name, Is.EqualTo(expected.Name));
            Assert.That(result.DescriptionStandard, Is.EqualTo(expected.DescriptionStandard));
            Assert.That(result.IsLegendary, Is.EqualTo(expected.IsLegendary));        }
    }
}