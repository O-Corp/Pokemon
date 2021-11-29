using System.Threading.Tasks;

namespace Pokemon.Rotomdex.Web.Api.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task GetPokemonDetails(string name);
    }
}