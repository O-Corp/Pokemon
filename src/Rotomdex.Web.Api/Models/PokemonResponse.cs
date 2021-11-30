namespace Rotomdex.Web.Api.Models
{
    public class PokemonResponse
    {
        public string Name { get; set; }
        
        public string DescriptionStandard { get; set; }
        
        public string Habitat { get; set; }
        
        public bool IsLegendary { get; set; }
    }
}