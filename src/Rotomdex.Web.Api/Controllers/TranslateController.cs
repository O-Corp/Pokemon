using System;
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

        public TranslateController(
            IPokemonService pokemonService,
            IMapper mapper)
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

            if (pokemon.Habitat.Equals("rare", StringComparison.CurrentCultureIgnoreCase) || pokemon.IsLegendary)
            {
                response.DescriptionStandard = "Fear is the path to the dark side";    
            }
            else
            {
                response.DescriptionStandard = "Give every man thy ear, but few thy voice";
            }
            
            return new OkObjectResult(response);
        }
    }
}