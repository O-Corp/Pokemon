using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Builders;

namespace Rotomdex.Integration.Decorators
{
    public class PokemonInfoDecorator : IPokemonDecorator
    {
        private readonly IPokemonDecorator _pokemonDecorator;
        private readonly IPokemonApiAdapter _pokemonApiAdapter;

        public PokemonInfoDecorator(
            IPokemonDecorator pokemonDecorator,
            IPokemonApiAdapter pokemonApiAdapter)
        {
            _pokemonDecorator = pokemonDecorator;
            _pokemonApiAdapter = pokemonApiAdapter;
        }
        
        public async Task<PokemonApiResponseBuilder> Decorate(PokeRequest request)
        {
            var builder = new PokemonApiResponseBuilder();
            var pokeApiResponse = await _pokemonApiAdapter.GetPokemon(request);

            if (pokeApiResponse != null)
            {
                request.Id = pokeApiResponse.Id.ToString();
                builder = await _pokemonDecorator.Decorate(request);
                builder
                    .WithId(pokeApiResponse.Id)
                    .WithName(pokeApiResponse.Name)
                    .WithSpecies(pokeApiResponse.Species);
            }

            return builder;
        }
    }
}