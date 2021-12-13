using System.Collections.Generic;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Integration.Builders
{
    public class PokemonApiResponseBuilder
    {
        private int _id;
        private string _name;
        private string _habitat;
        private bool _isLegendary;
        private List<Description> _descriptions;

        public PokemonApiResponseBuilder WithId(int value)
        {
            _id = value;
            return this;
        }

        public PokemonApiResponseBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public PokemonApiResponseBuilder WithHabitat(string value)
        {
            _habitat = value;
            return this;
        }

        public PokemonApiResponseBuilder WithIsLegendary(bool value)
        {
            _isLegendary = value;
            return this;
        }

        public PokemonApiResponseBuilder WithDescriptions(List<Description> value)
        {
            _descriptions = value;
            return this;
        }

        public PokemonApiResponse Build()
        {
            return new PokemonApiResponse
            {
                Id = _id,
                Name = _name,
                Habitat = _habitat,
                IsLegendary = _isLegendary,
                Descriptions = _descriptions
            };
        }
    }
}