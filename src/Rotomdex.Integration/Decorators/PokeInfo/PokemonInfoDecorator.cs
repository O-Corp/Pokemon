using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Builders;

namespace Rotomdex.Integration.Decorators.PokeInfo
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
            var pokeInfoResponse = await _pokemonApiAdapter.GetPokemon(request);

            if (pokeInfoResponse != null)
            {
                request.Id = pokeInfoResponse.Id.ToString();
                builder = await _pokemonDecorator.Decorate(request);
                builder
                    .WithId(pokeInfoResponse.Id)
                    .WithName(pokeInfoResponse.Name);
            }

            return builder;
        }
    }
}