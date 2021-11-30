using System.Threading.Tasks;
using Pokemon.Rotomdex.Web.Api.Adapters.Contracts;

namespace Pokemon.Rotomdex.Web.Api.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task<PokeApiResponse> GetPokemon(string name);
    }
}