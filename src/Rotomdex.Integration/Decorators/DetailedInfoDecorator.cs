using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Builders;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Integration.Decorators
{
    public class DetailedInfoDecorator : IPokemonDecorator
    {
        private readonly IPokemonApiAdapter _pokemonApiAdapter;

        public DetailedInfoDecorator(IPokemonApiAdapter pokemonApiAdapter)
        {
            _pokemonApiAdapter = pokemonApiAdapter;
        }
        
        public async Task<PokemonApiResponseBuilder> Decorate(PokeRequest request)
        {
            var builder = new PokemonApiResponseBuilder();
            var speciesDetails = await _pokemonApiAdapter.GetSpeciesDetails(request);
            speciesDetails.FlavorTextEntries = new List<Description>
            {
                GetDescriptionOrDefault(request, speciesDetails)
            };
            builder.WithSpeciesDetails(speciesDetails);
            return builder;
        }

        private static Description GetDescriptionOrDefault(PokeRequest request, SpeciesDetails speciesDetails)
        {
            var language = request.Language;
            var descriptions = speciesDetails.FlavorTextEntries;
            var description = descriptions.FirstOrDefault(x => x.Language.Name.Equals(language, StringComparison.CurrentCultureIgnoreCase));

            if (description != null)
            {
                return description;
            }

            const string defaultLanguage = "en";
            description = descriptions.FirstOrDefault(x => x.Language.Name.Equals(defaultLanguage, StringComparison.CurrentCultureIgnoreCase));
            return description;
        }
    }
}