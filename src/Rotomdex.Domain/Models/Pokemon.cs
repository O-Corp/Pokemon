using System.Globalization;
using Rotomdex.Domain.Exceptions;

namespace Rotomdex.Domain.Models
{
    public class Pokemon
    {
        private Pokemon(
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

        public static Pokemon Create(
            string name,
            string description,
            string habitat,
            bool isLegendary)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidPokemonException(nameof(Pokemon.Name), name);
            }
            
            var textInfo = new CultureInfo("en-GB", false).TextInfo;
            return new Pokemon(
                textInfo.ToTitleCase(name), 
                description, 
                textInfo.ToTitleCase(habitat),
                isLegendary);
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; }
        
        public string Description { get; private set; }
        
        public string Habitat { get; }
        
        public bool IsLegendary { get; }
    }
}