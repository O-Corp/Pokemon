using System;
using System.Threading.Tasks;
using Rotomdex.Domain.Models;
using Rotomdex.Integration.Factories;

namespace Rotomdex.Integration.Decorators
{
    public class YodaTranslationDecorator : ITranslationDecorator
    {
        private readonly ITranslationDecorator _decorator;
        private readonly ITranslatorFactory _translatorFactory;

        public YodaTranslationDecorator(
            ITranslationDecorator decorator,
            ITranslatorFactory translatorFactory)
        {
            _decorator = decorator;
            _translatorFactory = translatorFactory;
        }
        
        public async Task<Translation> Translate(Pokemon pokemon)
        {
            if (UseYodaTranslation(pokemon))
            {
                var translationsApi = _translatorFactory.Create(TranslationType.Yoda);
                var translationResponse = await translationsApi.Translate(pokemon.Description);
                return translationResponse == null 
                    ? null
                    : new Translation(translationResponse.Contents.Translated);
            }

            return await _decorator.Translate(pokemon);
        }
        
        private static bool UseYodaTranslation(Pokemon pokemon)
        {
            return pokemon.Habitat.Equals("rare", StringComparison.CurrentCultureIgnoreCase) || pokemon.IsLegendary;
        }
    }
}