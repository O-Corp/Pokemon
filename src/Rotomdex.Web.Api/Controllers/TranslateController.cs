using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Services;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class TranslateController
    {
        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;

        public TranslateController(IPokemonService pokemonService, IMapper mapper)
        {
            _pokemonService = pokemonService;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Post([FromBody] TranslationRequest request)
        {
            var pokemon = await _pokemonService.GetPokemon(new PokeRequest { Name = request.Name });
            var response = _mapper.Map<PokemonResponse>(pokemon);
            response.DescriptionStandard = "Ye Olde Shakespeare Talk";
            
            return new OkObjectResult(response);
        }
    }
}