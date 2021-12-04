using System;
using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Factories;

namespace Rotomdex.Integration.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IPokemonService _pokemonService;
        private readonly ITranslatorFactory _translatorFactory;

        public TranslationService(IPokemonService pokemonService, ITranslatorFactory translatorFactory)
        {
            _pokemonService = pokemonService;
            _translatorFactory = translatorFactory;
        }

        public async Task<Pokemon> Translate(PokeRequest request)
        {
            var pokemon = await _pokemonService.GetPokemon(request);
            pokemon.UpdateDescription(await GetTranslation(pokemon));

            return pokemon;
        }

        private async Task<string> GetTranslation(Pokemon pokemon)
        {
            var translatorAdapter = GetTranslationsApiAdapter(pokemon);
            var translationResponse = await translatorAdapter.Translate(pokemon.Description);
            
            return translationResponse != null 
                ? translationResponse.Contents.Translated 
                : pokemon.Description;
        }

        private ITranslationsApiAdapter GetTranslationsApiAdapter(Pokemon pokemon)
        {
            var translatorAdapter = _translatorFactory.Create(UseYodaTranslation(pokemon)
                ? TranslationType.Yoda
                : TranslationType.Shakespeare);
            return translatorAdapter;
        }

        private static bool UseYodaTranslation(Pokemon pokemon)
        {
            return pokemon.Habitat.Equals("rare", StringComparison.CurrentCultureIgnoreCase) || pokemon.IsLegendary;
        }
    }
}