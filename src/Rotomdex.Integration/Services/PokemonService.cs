using System;
using System.Linq;
using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Contracts;

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

            if (apiResponse != null)
            {
                // TODO: use automapper here?
                return new Pokemon(
                    apiResponse.Name,
                    GetDescriptionOrDefault(request, apiResponse),
                    apiResponse.SpeciesDetails.Habitat.Name,
                    apiResponse.SpeciesDetails.IsLegendary);                
            }

            return null;
        }

        private static string GetDescriptionOrDefault(PokeRequest request, PokeApiResponse apiResponse)
        {
            var language = request.Language;
            var descriptions = apiResponse.SpeciesDetails.FlavorTextEntries;
            var regionalDescription = descriptions.FirstOrDefault(x => x.Language.Name.Equals(language, StringComparison.CurrentCultureIgnoreCase));

            if (regionalDescription != null)
            {
                return regionalDescription.FlavourText;
            }

            const string defaultLanguage = "en";
            regionalDescription = descriptions.FirstOrDefault(x => x.Language.Name.Equals(defaultLanguage, StringComparison.CurrentCultureIgnoreCase));
            return regionalDescription.FlavourText;
        }
    }
}