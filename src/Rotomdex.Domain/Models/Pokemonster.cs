namespace Rotomdex.Domain.Models
{
    public class Pokemonster
    {
        public Pokemonster(
            string name,
            string description,
            string habitat,
            bool isLegendary)
        {
            Name = name;
            Description = description;
            Habitat = habitat;
            IsLegendary = isLegendary;
        }
        
        public string Name { get; }
        
        public string Description { get; }
        
        public string Habitat { get; }
        
        public bool IsLegendary { get; }
    }
}