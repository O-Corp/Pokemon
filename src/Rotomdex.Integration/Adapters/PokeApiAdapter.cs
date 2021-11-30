using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Rotomdex.Domain.Adapters;
using Rotomdex.Domain.Models;
using Rotomdex.Integration.Contracts;

namespace Rotomdex.Integration.Adapters
{
    public class PokeApiAdapter : IPokemonApiAdapter
    {
        private readonly HttpClient _httpClient;

        public PokeApiAdapter(HttpClient httpClient, Uri baseAddress)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<Pokemon> GetPokemon(string name)
        {
            var httpResponse = await _httpClient.GetAsync($"api/v2/pokemon/{name.ToLower()}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }
            
            var response = await httpResponse.Content.ReadFromJsonAsync<PokeApiResponse>();
            response.SpeciesDetails = await GetSpeciesDetails(response.Id);
            
            return new Pokemon(
                response.Name,
                response.SpeciesDetails.FlavorTextEntries[0].FlavourText,
                response.SpeciesDetails.Habitat.Name,
                response.SpeciesDetails.IsLegendary);
        }

        private async Task<SpeciesDetails> GetSpeciesDetails(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/v2/pokemon-species/{id}");
            return await httpResponse.Content.ReadFromJsonAsync<SpeciesDetails>();
        }
    }
}