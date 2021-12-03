using System;
using System.Collections.Generic;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Web.Api.ComponentTests
{
    public class PokeApiResponseBuilder
    {
        private const string EnglishDescription = "It was created by a scientist.";
        private const string FrenchDescription = "Il a été créé par un scientifique.";
        private const string EnglishLanguageCode = "en";
        private const string FrenchLanguageCode = "fr";
        private PokeApiResponse _response;

        public PokeApiResponseBuilder WithValidResponse()
        {
            _response = new PokeApiResponse
            {
                Id = 123,
                Name = "mewtwo",
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
                    }
                }
            };

            return this;
        }

        public PokeApiResponseBuilder WithName(string value)
        {
            _response.Name = value;
            return this;
        }

        public PokeApiResponseBuilder WithHabitat(string value)
        {
            _response.SpeciesDetails.Habitat.Name = value;
            return this;
        }

        public PokeApiResponseBuilder WithLegendary(bool value)
        {
            _response.SpeciesDetails.IsLegendary = value;
            return this;
        }

        public PokeApiResponseBuilder WithInvalidPokemon()
        {
            _response = null;
            return this;
        }

        public PokeApiResponse Build()
        {
            return _response;
        }
    }
}