using Microsoft.AspNetCore.Mvc;

namespace Pokemon.Rotomdex.Web.Api.Controllers
{
    [Route("rotomdex/v1/pokemon")]
    public class PokemonController : ControllerBase
    {
        [HttpGet]
        [Route("{name}")]
        public IActionResult Get([FromRoute] string name)
        {
            return Ok();
        }
    }
}