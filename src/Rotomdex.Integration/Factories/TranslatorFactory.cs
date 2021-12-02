using System.Collections.Generic;
using System.Linq;
using Rotomdex.Integration.Adapters;

namespace Rotomdex.Integration.Factories
{
    public class TranslatorFactory : ITranslatorFactory
    {
        private readonly IEnumerable<ITranslationsApiAdapter> _translationsApiAdapters;

        public TranslatorFactory(IEnumerable<ITranslationsApiAdapter> translationsApiAdapters)
        {
            _translationsApiAdapters = translationsApiAdapters;
        }

        public ITranslationsApiAdapter Create(TranslationType translationType)
        {
            return translationType switch
            {
                TranslationType.Yoda => _translationsApiAdapters.FirstOrDefault(x => x is YodaTranslatorAdapter),
                TranslationType.Shakespeare => _translationsApiAdapters.FirstOrDefault(x => x is ShakespeareTranslatorAdapter),
                _ => null // throw exception
            };
        }
    }
}