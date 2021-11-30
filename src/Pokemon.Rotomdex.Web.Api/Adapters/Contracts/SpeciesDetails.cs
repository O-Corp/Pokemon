using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokemon.Rotomdex.Web.Api.Adapters.Contracts
{
    public class SpeciesDetails
    {
        [JsonPropertyName("flavor_text_entries")]
        public List<Description> FlavorTextEntries { get; set; }
        
        public  Habitat Habitat { get; set; }
        
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}