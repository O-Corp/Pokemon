namespace Rotomdex.Integration.Contracts.PokeApi
{
    public class PokeResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Species Species { get; set; }
    }

    public class PokemonApiResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Species Species { get; set; }

        public SpeciesDetails SpeciesDetails { get; set; }
    }
}