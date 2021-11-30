using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Pokemon.Rotomdex.Web.Api.Adapters.Contracts;

namespace Pokemon.Rotomdex.Web.Api.Adapters
{
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
            return await httpResponse.Content.ReadFromJsonAsync<SpeciesDetails>();
        }
    }
}