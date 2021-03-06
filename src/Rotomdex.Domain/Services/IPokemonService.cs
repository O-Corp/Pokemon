using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Models;

namespace Rotomdex.Domain.Services
{
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemon(PokeRequest request);
    }
}