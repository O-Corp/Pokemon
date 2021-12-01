using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Services;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;

        public PokemonController(
            IPokemonService pokemonService,
            IMapper mapper)
        {
            _pokemonService = pokemonService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get([FromRoute] PokemonRequestFilter request)
        {
            var pokemon = await _pokemonService.GetPokemon(_mapper.Map<PokeRequest>(request));
            if (pokemon == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(_mapper.Map<PokemonResponse>(pokemon));
        }
    }
}