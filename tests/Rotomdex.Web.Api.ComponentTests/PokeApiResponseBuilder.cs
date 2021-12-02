using System;
using System.Collections.Generic;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Web.Api.ComponentTests
{
    public class PokeApiResponseBuilder
    {
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
                            FlavourText = "It was created by a scientist.",
                            Language = new Language { Name = "en" }
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

        public PokeApiResponse Build()
        {
            return _response;
        }
    }
}