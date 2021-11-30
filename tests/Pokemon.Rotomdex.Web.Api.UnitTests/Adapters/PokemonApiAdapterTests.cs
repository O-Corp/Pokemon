using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Pokemon.Rotomdex.Web.Api.Adapters;

namespace Pokemon.Rotomdex.Web.Api.UnitTests.Adapters
{
    [TestFixture]
    public class PokeApiStandardAdapterTests
    {
        private const string BaseAddress = "https://pokeapi.co";
        private const string PokemonName = "ditto";
        private const int PokemonId = 132;
        private PokeApiStandardAdapter _subject;
        private FakeHttpHandler _fakeHttpHandler;

        [SetUp]
        public async Task Setup()
        {
            var pokemonStandardJson = await File.ReadAllTextAsync("Data/pokemon_details.json");
            var pokemonSpeciesJson = await File.ReadAllTextAsync("Data/pokemon_species.json");
            
            _fakeHttpHandler = new FakeHttpHandler();
            _fakeHttpHandler.SetupResponse($"{BaseAddress}/api/v2/pokemon/{PokemonName}", pokemonStandardJson, HttpStatusCode.OK);
            _fakeHttpHandler.SetupResponse($"{BaseAddress}/api/v2/pokemon-species/{PokemonId}", pokemonSpeciesJson, HttpStatusCode.OK);
            _subject = new PokeApiStandardAdapter(new HttpClient(_fakeHttpHandler), new Uri(BaseAddress));
        }
        
        [Test]
        public async Task When_Request_Is_Sent_Then_The_Correct_Uri_Is_Used()
        {
            await _subject.GetPokemon(PokemonName);

            Assert.That(_fakeHttpHandler.HttpRequests[0].RequestUri.ToString(), Is.EqualTo($"{BaseAddress}/api/v2/pokemon/{PokemonName}"));
            Assert.That(_fakeHttpHandler.HttpRequests[1].RequestUri.ToString(), Is.EqualTo($"{BaseAddress}/api/v2/pokemon-species/{PokemonId}"));
        }
        
        [Test]
        public async Task When_Request_Is_Sent_Then_The_Correct_Response_Is_Returned()
        {
            var result = await _subject.GetPokemon(PokemonName);
            
            Assert.That(result.Id, Is.EqualTo(PokemonId));
            Assert.That(result.Name, Is.EqualTo(PokemonName));
            Assert.That(result.SpeciesDetails.Habitat.Name, Is.EqualTo("urban"));
            Assert.That(result.SpeciesDetails.IsLegendary, Is.False);
            Assert.That(result.Species.Url.ToString(), Is.EqualTo($"{BaseAddress}/api/v2/pokemon-species/{PokemonId}/"));
            Assert.That(result.SpeciesDetails.FlavorTextEntries, Is.Not.Empty);
        }

        [Test]
        public async Task When_Request_Is_Sent_For_Non_Existent_Pokemon_Then_Response_Is_Null()
        {
            const string nonExistentPokemon = "XXX";
            
            _fakeHttpHandler.SetupResponse($"{BaseAddress}/api/v2/pokemon/{nonExistentPokemon}", null, HttpStatusCode.NotFound);

            var result = await _subject.GetPokemon(nonExistentPokemon);

            Assert.That(result, Is.Null);
        }
    }
    
    public class FakeHttpHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, Expectation> _responses;

        public FakeHttpHandler()
        {
            _responses = new Dictionary<string, Expectation>();
            HttpRequests = new List<HttpRequestMessage>();
        }

        public void SetupResponse(string uri, string jsonResponse, HttpStatusCode httpStatusCode)
        {
            _responses.Add(uri, new Expectation(jsonResponse, httpStatusCode));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequests.Add(request);
            
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            if (_responses.TryGetValue(request.RequestUri.ToString(), out var expectation))
            {
                if (!string.IsNullOrWhiteSpace(expectation.Json))
                {
                    httpResponseMessage.Content = new StringContent(expectation.Json);    
                }
                
                httpResponseMessage.StatusCode = expectation.HttpStatusCode;
            }

            return Task.FromResult(httpResponseMessage);
        }

        public List<HttpRequestMessage> HttpRequests { get; }
    }

    public class Expectation
    {
        public Expectation(string json, HttpStatusCode httpStatusCode)
        {
            Json = json;
            HttpStatusCode = httpStatusCode;
        }
        
        public string Json { get; }
        
        public HttpStatusCode HttpStatusCode { get; }
    }
}