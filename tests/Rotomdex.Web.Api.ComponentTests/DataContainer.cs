using Moq;
using Rotomdex.Domain.Adapters;

namespace Rotomdex.Web.Api.ComponentTests
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