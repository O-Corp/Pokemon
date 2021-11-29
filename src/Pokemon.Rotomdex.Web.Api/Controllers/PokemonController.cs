using Microsoft.AspNetCore.Mvc;
using Pokemon.Rotomdex.Web.Api.Models;

namespace Pokemon.Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class PokemonController : ControllerBase
    {
        [HttpGet]
        [Route("{name}")]
        public IActionResult Get([FromRoute] string name)
        {
            var response = new PokemonDetails
            {
                Habitat = "Rare",
                Name = "Mewtwo",
                DescriptionStandard = "It was created by a scientist.",
                IsLegendary = true
            };
            
            return new OkObjectResult(response);
        }
    }
}