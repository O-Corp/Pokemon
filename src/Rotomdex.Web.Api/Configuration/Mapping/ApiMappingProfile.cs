﻿using AutoMapper;
using Rotomdex.Domain.DTOs;
using Rotomdex.Web.Api.Models;

namespace Rotomdex.Web.Api.Configuration.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<PokemonRequestFilter, PokeRequest>();
            CreateMap<TranslationRequest, PokeRequest>()
                .ForMember(dest => dest.Language, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore());
        }
    }
}