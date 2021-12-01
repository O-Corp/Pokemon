using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;
using Rotomdex.Web.Api.Configuration;

namespace Rotomdex.IntegrationTests.Adapters
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

        [TestCase("MEWTWO")]
        [TestCase("meWtwO")]
        public async Task When_Retrieving_Pokemon_Details_Then_Correct_Response_Is_Returned(string name)
        {
            var result = await _subject.GetPokemon(new PokeRequest { Name = name });
            
            Assert.That(result.Name, Is.EqualTo("mewtwo"));
            Assert.That(result.Description, Is.Not.Null);
            Assert.That(result.Habitat, Is.EqualTo("rare"));
            Assert.That(result.IsLegendary, Is.True);
        }
        
        [Test]
        public async Task When_Retrieving_Pokemon_Details_For_Non_Existent_Pokemon_Then_Return_Null()
        {
            var result = await _subject.GetPokemon(new PokeRequest { Name = "xxx" });
            Assert.That(result, Is.Null);
        }
    }
}