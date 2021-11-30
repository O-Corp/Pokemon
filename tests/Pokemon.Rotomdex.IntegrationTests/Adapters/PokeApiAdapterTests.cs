using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Pokemon.Rotomdex.Web.Api.Adapters;
using Pokemon.Rotomdex.Web.Api.Configuration;

namespace Pokemon.Rotomdex.IntegrationTests.Adapters
{
    [TestFixture]
    public class PokeApiAdapterTests
    {
        private PokeApiAdapter _subject;
        private PokeApiSettings _pokeApiSettings;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
            
            _pokeApiSettings = new PokeApiSettings();
            config.Bind(nameof(PokeApiSettings), _pokeApiSettings);

            _subject = new PokeApiAdapter(new HttpClient(), _pokeApiSettings.BaseAddress);
        }

        [Test]
        public async Task When_Retrieving_Pokemon_Details_Then_Correct_Response_Is_Returned()
        {
            var result = await _subject.GetPokemon("mewtwo");
            var expectedUri = new Uri($"{_pokeApiSettings.BaseAddress}api/v2/pokemon-species/{result.Id}/");
            
            Assert.That(result.Id, Is.EqualTo(150));
            Assert.That(result.Name, Is.EqualTo("mewtwo"));
            Assert.That(result.Species.Url.ToString(), Is.EqualTo(expectedUri.ToString()));
            Assert.That(result.SpeciesDetails.Habitat.Name, Is.EqualTo("rare"));
            Assert.That(result.SpeciesDetails.IsLegendary, Is.True);
            Assert.That(result.SpeciesDetails.FlavorTextEntries, Is.Not.Empty);
        }
        
        [Test]
        public async Task When_Retrieving_Pokemon_Details_For_Non_Existent_Pokemon_Then_Return_Null()
        {
            var result = await _subject.GetPokemon("XXX");
            Assert.That(result, Is.Null);
        }
    }
}