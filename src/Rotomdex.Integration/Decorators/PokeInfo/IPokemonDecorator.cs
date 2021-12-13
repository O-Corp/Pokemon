using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Builders;

namespace Rotomdex.Integration.Decorators.PokeInfo
{
    public interface IPokemonDecorator
    {
        Task<PokemonApiResponseBuilder> Decorate(PokeRequest request);
    }
}