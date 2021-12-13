using System.Collections.Generic;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Testing.Common.Helpers
{
    public class PokeSpeciesDetailsResponseBuilder
    {
        private const string EnglishDescription = "It was created by a scientist.";
        private const string FrenchDescription = "Il a été créé par un scientifique.";
        private const string EnglishLanguageCode = "en";
        private const string FrenchLanguageCode = "fr";
        private SpeciesDetails _speciesDetails;

        public PokeSpeciesDetailsResponseBuilder()
        {
            _speciesDetails = new SpeciesDetails();
        }

        public PokeSpeciesDetailsResponseBuilder WithValidResponse()
        {
            _speciesDetails = new SpeciesDetails
            {
                Habitat = new Habitat { Name = "rare" },
                IsLegendary = true,
                FlavorTextEntries = new List<Description>
                {
                    new()
                    {
                        FlavourText = EnglishDescription,
                        Language = new Language { Name = EnglishLanguageCode }
                    },
                    new()
                    {
                        FlavourText = FrenchDescription,
                        Language = new Language { Name = FrenchLanguageCode }
                    }
                }
            };
            return this;
        }

        public PokeSpeciesDetailsResponseBuilder WithHabitat(string value)
        {
            _speciesDetails.Habitat.Name = value;
            return this;
        }

        public PokeSpeciesDetailsResponseBuilder WithLegendary(bool value)
        {
            _speciesDetails.IsLegendary = value;
            return this;
        }

        public PokeSpeciesDetailsResponseBuilder WithDescription(string description)
        {
            const int englishDescription = 0;
            _speciesDetails.FlavorTextEntries[englishDescription].FlavourText = description;
            return this;
        }

        public SpeciesDetails Build()
        {
            return _speciesDetails;
        }
    }
}