using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pokemon.Rotomdex.Web.Api.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task<PokeApiResponse> GetPokemon(string name);
    }

    public class PokeApiAdapter : IPokemonApiAdapter
    {
        private readonly HttpClient _httpClient;

        public PokeApiAdapter(HttpClient httpClient, Uri baseAddress)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<PokeApiResponse> GetPokemon(string name)
        {
            var httpResponse = await _httpClient.GetAsync($"api/v2/pokemon/{name}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }
            
            var response = await httpResponse.Content.ReadFromJsonAsync<PokeApiResponse>();
            response.SpeciesDetails = await GetSpeciesDetails(response.Id);
            return response;
        }

        private async Task<SpeciesDetails> GetSpeciesDetails(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/v2/pokemon-species/{id}");
            var details = await httpResponse.Content.ReadFromJsonAsync<SpeciesDetails>();

            return details;
        }
    }
    
    public class PokeApiResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Species Species { get; set; }

        public SpeciesDetails SpeciesDetails { get; set; }
    }

    public class Species
    {
        public Uri Url { get; set; }
    }

    public class SpeciesDetails
    {
        [JsonPropertyName("flavor_text_entries")]
        public List<Description> FlavorTextEntries { get; set; }
        
        public  Habitat Habitat { get; set; }
        
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }

    public class Language
    {
        public string Name { get; set; }
    }

    public class Habitat
    {
        public string Name { get; set; }
    }
    
    public class Description
    {
        [JsonPropertyName("flavor_text")]
        public string FlavourText { get; set; }
        
        public Language Language { get; set; }
    }
}