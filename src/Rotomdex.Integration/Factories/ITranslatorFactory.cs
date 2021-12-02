using Rotomdex.Integration.Adapters;

namespace Rotomdex.Integration.Factories
{
    public interface ITranslatorFactory
    {
        ITranslationsApiAdapter Create(TranslationType translationType);
    }
}