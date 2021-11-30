using System.Threading.Tasks;
using Rotomdex.Domain.Models;

namespace Rotomdex.Domain.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task<Pokemon> GetPokemon(string name);
    }
}