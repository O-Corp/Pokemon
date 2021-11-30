using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rotomdex.Domain.Adapters;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonApiAdapter _pokemonApiAdapter;

        public PokemonController(IPokemonApiAdapter pokemonApiAdapter)
        {
            _pokemonApiAdapter = pokemonApiAdapter;
        }
        
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            var pokemon = await _pokemonApiAdapter.GetPokemon(name);
            var response = new PokemonDetails
            {
                Habitat = pokemon.Habitat,
                Name = pokemon.Name,
                DescriptionStandard = pokemon.Description,
                IsLegendary = pokemon.IsLegendary
            };
            
            return new OkObjectResult(response);
        }
    }
}