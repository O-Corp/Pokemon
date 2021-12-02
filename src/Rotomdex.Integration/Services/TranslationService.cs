using System;
using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Factories;

namespace Rotomdex.Integration.Services
{
    public interface ITranslationService
    {
        Task<Pokemon> Translate(PokeRequest request);
    }
    
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
            ITranslationsApiAdapter translatorAdapter;
            
            if (pokemon.Habitat.Equals("rare", StringComparison.CurrentCultureIgnoreCase) || pokemon.IsLegendary)
            {
                translatorAdapter= _translatorFactory.Create(TranslationType.Yoda);
            }
            else
            {
                translatorAdapter = _translatorFactory.Create(TranslationType.Shakespeare);
            }
            
            var translationResponse = await translatorAdapter.Translate(pokemon.Description);
            pokemon.Description = translationResponse.Contents.Translated;

            return pokemon;
        }
    }
}