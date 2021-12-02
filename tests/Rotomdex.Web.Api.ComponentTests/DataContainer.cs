using Moq;
using Rotomdex.Integration.Adapters;

namespace Rotomdex.Web.Api.ComponentTests
{
    public class DataContainer
    {
        public DataContainer()
        {
            ApiAdapter = Mock.Of<IPokemonApiAdapter>();
            YodaTranslationsAdapter = Mock.Of<ITranslationsApiAdapter>();
            ShakespeareTranslationsAdapter = Mock.Of<ITranslationsApiAdapter>();
        }
        
        public IPokemonApiAdapter ApiAdapter { get; set; }
        
        public ITranslationsApiAdapter YodaTranslationsAdapter { get; set; }
        
        public ITranslationsApiAdapter ShakespeareTranslationsAdapter { get; set; }
    }
}