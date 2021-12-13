using System.Linq;
using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Decorators.PokeInfo;

namespace Rotomdex.Integration.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonDecorator _pokemonDecorator;

        public PokemonService(IPokemonDecorator pokemonDecorator)
        {
            _pokemonDecorator = pokemonDecorator;
        }
        
        public async Task<Pokemon> GetPokemon(PokeRequest request)
        {
            var builder = await _pokemonDecorator.Decorate(request);
            var apiResponse = builder.Build();

            if (!string.IsNullOrWhiteSpace(apiResponse.Name))
            {
                return Pokemon.Create(
                    apiResponse.Name,
                    apiResponse.Descriptions.First().FlavourText,
                    apiResponse.Habitat,
                    apiResponse.IsLegendary);                
            }

            return null;
        }
    }
}