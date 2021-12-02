using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Services;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class TranslateController
    {
        private readonly ITranslationService _translationService;
        private readonly IMapper _mapper;

        public TranslateController(
            ITranslationService translationService,
            IMapper mapper)
        {
            _translationService = translationService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Post([FromBody] TranslationRequest request)
        {
            var response = await _translationService.Translate(_mapper.Map<PokeRequest>(request));
            return new OkObjectResult(_mapper.Map<PokemonResponse>(response));
        }
    }
}