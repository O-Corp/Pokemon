using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Adapters;

namespace Rotomdex.Integration.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApiAdapter _pokemonApiAdapter;

        public PokemonService(IPokemonApiAdapter pokemonApiAdapter)
        {
            _pokemonApiAdapter = pokemonApiAdapter;
        }
        
        public async Task<Pokemon> GetPokemon(PokeRequest request)
        {
            var apiResponse = await _pokemonApiAdapter.GetPokemon(request);
            
            // TODO: use automapper here?
            return new Pokemon(
                apiResponse.Name,
                apiResponse.SpeciesDetails.FlavorTextEntries[0].FlavourText, // TODO: tease this logic out
                apiResponse.SpeciesDetails.Habitat.Name,
                apiResponse.SpeciesDetails.IsLegendary);
        }
    }
}