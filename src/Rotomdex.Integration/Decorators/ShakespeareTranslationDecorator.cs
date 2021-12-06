using System.Threading.Tasks;
using Rotomdex.Domain.Models;
using Rotomdex.Integration.Factories;

namespace Rotomdex.Integration.Decorators
{
    public class ShakespeareTranslationDecorator : ITranslationDecorator
    {
        private readonly ITranslatorFactory _factory;

        public ShakespeareTranslationDecorator(ITranslatorFactory factory)
        {
            _factory = factory;
        }
        
        public async Task<Translation> Translate(Pokemon pokemon)
        {
            var translationsApi = _factory.Create(TranslationType.Shakespeare);
            var response = await translationsApi.Translate(pokemon.Description);
            return response == null 
                ? null 
                : new Translation(response.Contents.Translated);
        }
    }
}