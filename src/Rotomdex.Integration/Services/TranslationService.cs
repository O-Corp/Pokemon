using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Decorators;

namespace Rotomdex.Integration.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IPokemonService _pokemonService;
        private readonly ITranslationDecorator _translationDecorator;

        public TranslationService(
            IPokemonService pokemonService,
            ITranslationDecorator translationDecorator)
        {
            _pokemonService = pokemonService;
            _translationDecorator = translationDecorator;
        }

        public async Task<Pokemon> Translate(PokeRequest request)
        {
            var pokemon = await _pokemonService.GetPokemon(request);
            pokemon.UpdateDescription((await _translationDecorator.Translate(pokemon)).ToString());

            return pokemon;
        }
    }
}