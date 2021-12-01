using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rotomdex.Domain.Adapters;
using Rotomdex.Domain.DTOs;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonApiAdapter _pokemonApiAdapter;
        private readonly IMapper _mapper;

        public PokemonController(
            IPokemonApiAdapter pokemonApiAdapter,
            IMapper mapper)
        {
            _pokemonApiAdapter = pokemonApiAdapter;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get([FromRoute] PokemonRequestFilter request)
        {
            var pokemon = await _pokemonApiAdapter.GetPokemon(_mapper.Map<PokeRequest>(request));
            if (pokemon == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(_mapper.Map<PokemonResponse>(pokemon));
        }
    }
}