using System.Threading.Tasks;
using Pokemon.Rotomdex.Domain.Models;

namespace Pokemon.Rotomdex.Domain.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task<Pokemonster> GetPokemon(string name);
    }
}