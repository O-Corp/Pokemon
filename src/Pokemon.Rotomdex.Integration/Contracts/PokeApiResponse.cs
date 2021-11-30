namespace Pokemon.Rotomdex.Integration.Contracts
{
    public class PokeApiResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Species Species { get; set; }

        public SpeciesDetails SpeciesDetails { get; set; }
    }
}