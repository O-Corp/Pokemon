using Moq;
using Pokemon.Rotomdex.Domain.Adapters;

namespace Pokemon.Rotomdex.Web.Api.ComponentTests
{
    public class DataContainer
    {
        public DataContainer()
        {
            ApiAdapter = Mock.Of<IPokemonApiAdapter>();
        }
        
        public IPokemonApiAdapter ApiAdapter { get; set; }
    }
}