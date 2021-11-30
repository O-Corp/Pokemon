namespace Rotomdex.Domain.Models
{
    public class Pokemon
    {
        public Pokemon(
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