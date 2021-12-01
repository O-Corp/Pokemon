using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;

namespace Rotomdex.Domain.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task<Pokemon> GetPokemon(PokeRequest request);
    }
}