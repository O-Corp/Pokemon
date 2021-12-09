using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;

namespace Rotomdex.IntegrationTests.Adapters
{
    [TestFixture]
    public class PokeApiAdapterTests
    {
        private PokeApiAdapter _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new PokeApiAdapter(new HttpClient
            {
                BaseAddress = TestConfiguration.PokeApiSettings.BaseAddress
            });
        }

        [TestCase("MEWTWO")]
        [TestCase("meWtwO")]
        public async Task When_Retrieving_Pokemon_Details_Then_Correct_Response_Is_Returned(string name)
        {
            var result = await _subject.GetPokemon( new PokeRequest { Name = name });
            
            Assert.That(result.Name, Is.EqualTo("mewtwo"));
            Assert.That(result.SpeciesDetails.FlavorTextEntries, Is.Not.Empty);
            Assert.That(result.SpeciesDetails.Habitat.Name, Is.EqualTo("rare"));
            Assert.That(result.SpeciesDetails.IsLegendary, Is.True);
        }
        
        [Test]
        public async Task When_Retrieving_Pokemon_Details_For_Non_Existent_Pokemon_Then_Return_Null()
        {
            var result = await _subject.GetSpeciesDetails(new PokeRequest { Name = "xxx" });
            Assert.That(result, Is.Null);
        }
    }
}