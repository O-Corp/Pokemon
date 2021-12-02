namespace Rotomdex.Integration.Contracts.PokeApi
{
    public class PokeApiResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Species Species { get; set; }

        public SpeciesDetails SpeciesDetails { get; set; }
    }
}