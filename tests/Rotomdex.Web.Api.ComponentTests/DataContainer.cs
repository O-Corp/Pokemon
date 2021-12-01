using Moq;
using Rotomdex.Integration.Adapters;

namespace Rotomdex.Web.Api.ComponentTests
{
    public class DataContainer
    {
        public DataContainer()
        {
            ApiAdapter = Mock.Of<IPokemonApiAdapter>();
            TranslationsAdapter = Mock.Of<ITranslationsApiAdapter>();
        }
        
        public IPokemonApiAdapter ApiAdapter { get; set; }
        
        public ITranslationsApiAdapter TranslationsAdapter { get; set; }
    }
}