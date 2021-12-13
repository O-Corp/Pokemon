using System.Collections.Generic;

namespace Rotomdex.Integration.Contracts.PokeApi
{
    public class PokemonApiResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Habitat { get; set; }
        
        public bool IsLegendary { get; set; }
        
        public List<Description> Descriptions { get; set; }
    }
}