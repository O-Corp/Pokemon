using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Integration.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task<PokeApiResponse> GetPokemon(PokeRequest request);
    }
}