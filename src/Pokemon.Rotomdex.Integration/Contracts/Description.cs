using System.Text.Json.Serialization;

namespace Pokemon.Rotomdex.Integration.Contracts
{
    public class Description
    {
        [JsonPropertyName("flavor_text")]
        public string FlavourText { get; set; }
        
        public Language Language { get; set; }
    }
}