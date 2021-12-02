using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Factories;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class TranslateController
    {
        private readonly IPokemonService _pokemonService;
        private readonly ITranslatorFactory _translatorFactory;
        private readonly IMapper _mapper;

        public TranslateController(
            IPokemonService pokemonService,
            ITranslatorFactory translatorFactory,
            IMapper mapper)
        {
            _pokemonService = pokemonService;
            _translatorFactory = translatorFactory;
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
                var translator = _translatorFactory.Create(TranslationType.Yoda);
                var foo = await translator.Translate(pokemon.Description);
                response.DescriptionStandard = foo.Contents.Translated;
            }
            else
            {
                var translator = _translatorFactory.Create(TranslationType.Shakespeare);
                var foo = await translator.Translate(pokemon.Description);
                response.DescriptionStandard = foo.Contents.Translated;
            }

            return new OkObjectResult(response);
        }
    }
}