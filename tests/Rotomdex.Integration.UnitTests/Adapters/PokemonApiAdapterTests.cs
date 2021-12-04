using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Exceptions;
using Rotomdex.Integration.Adapters;
using Rotomdex.Testing.Common.Fakes;

namespace Rotomdex.Integration.UnitTests.Adapters
{
    [TestFixture]
    public class PokeApiAdapterTests
    {
        private const string BaseAddress = "https://pokeapi.co";
        private const string PokemonName = "ditto";
        private const int PokemonId = 132;
        private FakePokeApiHttpHandler _fakePokeApiHttpHandler;
        private PokeApiAdapter _subject;

        [SetUp]
        public async Task Setup()
        {
            _fakePokeApiHttpHandler = new FakePokeApiHttpHandler();
            await _fakePokeApiHttpHandler.SetupResponse($"/api/v2/pokemon/{PokemonName}", PokeApiResponses.PokeApi_Details_Response);
            await _fakePokeApiHttpHandler.SetupResponse($"/api/v2/pokemon-species/{PokemonId}", PokeApiResponses.PokeApi_Species_Response);
            _subject = new PokeApiAdapter(new HttpClient(_fakePokeApiHttpHandler), new Uri(BaseAddress));
        }
        
        [Test]
        public async Task When_Request_Is_Sent_Then_The_Correct_Uri_Is_Used()
        {
            await _subject.GetPokemon(new PokeRequest { Name = PokemonName });

            Assert.That(_fakePokeApiHttpHandler.HttpRequests[0].RequestUri.ToString(), Is.EqualTo($"{BaseAddress}/api/v2/pokemon/{PokemonName}"));
            Assert.That(_fakePokeApiHttpHandler.HttpRequests[1].RequestUri.ToString(), Is.EqualTo($"{BaseAddress}/api/v2/pokemon-species/{PokemonId}"));
        }

        [TestCase("en", "It can freely recombine its own cellular structure to\ntransform into other life-forms.")]
        [TestCase("fr", "Il a la capacité de modifier sa\nstructure cellulaire pour prendre\nl’apparence de ce qu’il voit.")]
        public async Task When_Valid_Request_Is_Sent_With_Language_Filter_Then_Correct_Response_Is_Returned(string language, string expectedDescription)
        {
            var result = await _subject.GetPokemon(new PokeRequest
            {
                Name = PokemonName,
                Language = language
            });
            
            Assert.That(result.Name, Is.EqualTo(PokemonName));
            Assert.That(result.SpeciesDetails.Habitat.Name, Is.EqualTo("urban"));
            Assert.That(result.SpeciesDetails.IsLegendary, Is.False);
            Assert.That(result.SpeciesDetails.FlavorTextEntries, Is.Not.Empty);
        }
        
        [Test]
        public async Task When_Request_Is_Sent_For_Pokemon_Does_Not_Exist_Then_Response_Is_Null()
        {
            const string nonExistentPokemon = "xxx";
            _fakePokeApiHttpHandler.SetupResponse($"/api/v2/pokemon/{nonExistentPokemon}", HttpStatusCode.NotFound);

            var result = await _subject.GetPokemon(new PokeRequest
            {
                Name = nonExistentPokemon
            });

            Assert.That(result, Is.Null);
        }

        [Test]
        public void When_A_Transient_Error_Occurs_Then_Correct_Exception_Is_Thrown()
        {
            _subject = new PokeApiAdapter(new HttpClient(new ErrorHttpMessageHandler()), new Uri("http://foo.com/"));
            Assert.ThrowsAsync<ThirdPartyUnavailableException>(async () => await _subject.GetPokemon(new PokeRequest { Name = "dragonite" }));
        }
    }
}