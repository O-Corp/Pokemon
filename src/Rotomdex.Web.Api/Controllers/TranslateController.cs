using Microsoft.AspNetCore.Mvc;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class TranslateController
    {
        [HttpPost]
        [Route("translate")]
        public IActionResult Post(TranslationRequest request)
        {
            return new OkObjectResult(new PokemonResponse
            {
                Habitat = "cave",
                Name = "mewtwo",
                DescriptionStandard = "Ye Olde Shakespeare Talk",
                IsLegendary = true
            });
        }
    }
}