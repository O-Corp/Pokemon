using System;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Testing.Common.Helpers
{
    public class PokeInfoResponseBuilder
    {
        private PokeInfoResponse _pokeInfoResponse;

        public PokeInfoResponseBuilder()
        {
            _pokeInfoResponse = new PokeInfoResponse();
        }
        
        public PokeInfoResponseBuilder WithValidPokeInfoResponse()
        {
            _pokeInfoResponse = new PokeInfoResponse
            {
                Id = 123,
                Name = "mewtwo",
                Species = new Species { Url = new Uri("http://foo.com/species/123") }
            };

            return this;
        }
        
        public PokeInfoResponseBuilder WithName(string value)
        {
            _pokeInfoResponse.Name = value;
            return this;
        }

        public PokeInfoResponse Build()
        {
            return _pokeInfoResponse;
        }
    }
}