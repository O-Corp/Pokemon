using System.Threading.Tasks;
using Rotomdex.Domain.Models;

namespace Rotomdex.Integration.Decorators.Translation
{
    public class DefaultTranslationDecorator : ITranslationDecorator
    {
        private readonly ITranslationDecorator _decorator;

        public DefaultTranslationDecorator(ITranslationDecorator decorator)
        {
            _decorator = decorator;
        }

        public async Task<Domain.Models.Translation> Translate(Pokemon pokemon)
        {
            return await GetTranslationOrUseDefaultDescription(pokemon);
        }

        private async Task<Domain.Models.Translation> GetTranslationOrUseDefaultDescription(Pokemon pokemon)
        {
            var defaultDescription = pokemon.Description;
            var translation = await _decorator.Translate(pokemon);
            return translation ?? new Domain.Models.Translation(defaultDescription);
        }
    }
}