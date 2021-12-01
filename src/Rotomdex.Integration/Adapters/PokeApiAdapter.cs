using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Rotomdex.Domain.Adapters;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Exceptions;
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

        public async Task<Pokemon> GetPokemon(PokeRequest request)
        {
            try
            {
                var apiResponse = await GetPokemonDetails(request.Name);
                if (apiResponse != null)
                {
                    apiResponse.SpeciesDetails = await GetSpeciesDetails(apiResponse.Id);
                    
                    // TODO: use automapper here?
                    return new Pokemon(
                        apiResponse.Name,
                        apiResponse.SpeciesDetails.FlavorTextEntries[0].FlavourText, // TODO: tease this logic out
                        apiResponse.SpeciesDetails.Habitat.Name,
                        apiResponse.SpeciesDetails.IsLegendary);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ThirdPartyUnavailableException("PokeApi", e);
            }
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var transientStatusCodes = new[]
            {
                HttpStatusCode.InternalServerError,
                HttpStatusCode.GatewayTimeout
            };

            return Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult<HttpResponseMessage>(r => transientStatusCodes.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                });
        }

        private async Task<PokeApiResponse> GetPokemonDetails(string name)
        {
            var httpResponse = await GetRetryPolicy().ExecuteAsync(async () => await _httpClient.GetAsync($"api/v2/pokemon/{name.ToLower()}"));
            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }
            
            return await httpResponse.Content.ReadFromJsonAsync<PokeApiResponse>();
        }

        private async Task<SpeciesDetails> GetSpeciesDetails(int id)
        {
            var httpResponse = await GetRetryPolicy().ExecuteAsync(async () => await _httpClient.GetAsync($"api/v2/pokemon-species/{id}"));
            return await httpResponse.Content.ReadFromJsonAsync<SpeciesDetails>();
        }
    }
}