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
        
        public void UpdateDescription(string description)
        {
            Description = description;
        }
        
        public string Name { get; }
        
        public string Description { get; private set; }
        
        public string Habitat { get; }
        
        public bool IsLegendary { get; }
    }
}