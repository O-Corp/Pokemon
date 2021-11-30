using AutoMapper;
using Rotomdex.Domain.Models;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Configuration.Mapping
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<Pokemon, PokemonResponse>()
                .ConvertUsing<PokemonConverterToPokemonResponse>();
        }
        
        private class PokemonConverterToPokemonResponse : ITypeConverter<Pokemon, PokemonResponse>
        {
            public PokemonResponse Convert(Pokemon source, PokemonResponse destination, ResolutionContext context)
            {
                return new PokemonResponse
                {
                    Habitat = source.Habitat,
                    Name = source.Name,
                    DescriptionStandard = source.Description,
                    IsLegendary = source.IsLegendary
                };
            }
        }
    }
}