using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Integration.Builders
{
    public class PokemonApiResponseBuilder
    {
        private int _id;
        private string _name;
        private SpeciesDetails _speciesDetails;
        private Species _species;

        public PokemonApiResponseBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public PokemonApiResponseBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        
        public PokemonApiResponseBuilder WithSpecies(Species value)
        {
            _species = value;
            return this;
        }

        public PokemonApiResponseBuilder WithSpeciesDetails(SpeciesDetails value)
        {
            _speciesDetails = value;
            return this;
        }

        public PokemonApiResponse Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                return null;
            }
            
            return new PokemonApiResponse
            {
                Id = _id,
                Name = _name,
                Species = _species,
                SpeciesDetails = _speciesDetails
            };
        }
    }
}