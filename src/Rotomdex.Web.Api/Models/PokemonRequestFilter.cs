using Microsoft.AspNetCore.Mvc;

namespace Rotomdex.Web.Api.Models
{
    public class PokemonRequestFilter
    {
        [FromRoute]
        public string Name { get; set; }
        
        [FromQuery(Name = "lang")]
        public string Language { get; set; }
        
        [FromQuery(Name = "version")]
        public string Version { get; set; }
    }
}