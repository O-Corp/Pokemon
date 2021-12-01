﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Exceptions;
using Rotomdex.Integration.Adapters;

namespace Rotomdex.Integration.UnitTests.Adapters
{
    [TestFixture]
    public class PokeApiAdapterTests
    {
        private const string BaseAddress = "https://pokeapi.co";
        private const string PokemonName = "ditto";
        private const int PokemonId = 132;
        private PokeApiAdapter _subject;
        private FakeHttpHandler _fakeHttpHandler;

        [SetUp]
        public async Task Setup()
        {
            var pokemonStandardJson = await File.ReadAllTextAsync("Data/pokemon_details.json");
            var pokemonSpeciesJson = await File.ReadAllTextAsync("Data/pokemon_species.json");
            
            _fakeHttpHandler = new FakeHttpHandler();
            _fakeHttpHandler.SetupResponse($"{BaseAddress}/api/v2/pokemon/{PokemonName}", pokemonStandardJson, HttpStatusCode.OK);
            _fakeHttpHandler.SetupResponse($"{BaseAddress}/api/v2/pokemon-species/{PokemonId}", pokemonSpeciesJson, HttpStatusCode.OK);
            _subject = new PokeApiAdapter(new HttpClient(_fakeHttpHandler), new Uri(BaseAddress));
        }
        
        [Test]
        public async Task When_Request_Is_Sent_Then_The_Correct_Uri_Is_Used()
        {
            await _subject.GetPokemon(new PokeRequest { Name = PokemonName });

            Assert.That(_fakeHttpHandler.HttpRequests[0].RequestUri.ToString(), Is.EqualTo($"{BaseAddress}/api/v2/pokemon/{PokemonName}"));
            Assert.That(_fakeHttpHandler.HttpRequests[1].RequestUri.ToString(), Is.EqualTo($"{BaseAddress}/api/v2/pokemon-species/{PokemonId}"));
        }
        
        [TestCase("en", "It can freely recombine its own cellular structure to\ntransform into other life-forms.")]
        [TestCase("fr", "Il a la capacité de modifier sa\nstructure cellulaire pour prendre\nl’apparence de ce qu’il voit.")]
        public async Task When_Valid_Request_Is_Sent_Then_Correct_Response_Is_Returned(string language, string expectedDescription)
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
        public async Task When_Invalid_Request_Is_Sent_Then_Response_Is_Null()
        {
            const string nonExistentPokemon = "xxx";
            
            _fakeHttpHandler.SetupResponse($"{BaseAddress}/api/v2/pokemon/{nonExistentPokemon}", null, HttpStatusCode.NotFound);

            var result = await _subject.GetPokemon(new PokeRequest { Name = nonExistentPokemon });

            Assert.That(result, Is.Null);
        }

        [Test]
        public void When_A_Transient_Error_Occurs_Then_Correct_Exception_Is_Thrown()
        {
            _subject = new PokeApiAdapter(new HttpClient(new ErrorHttpHandler()), new Uri("http://foo.com/"));
            Assert.ThrowsAsync<ThirdPartyUnavailableException>(async () => await _subject.GetPokemon(new PokeRequest { Name = "dragonite" }));
        }
    }
}