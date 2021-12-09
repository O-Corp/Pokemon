using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Retry;
using Rotomdex.Domain.DTOs;
using Rotomdex.Domain.Exceptions;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Integration.Adapters
{
    public interface IPokemonApiAdapter
    {
        Task<PokemonApiResponse> GetPokemon(PokeRequest request);

        Task<SpeciesDetails> GetSpeciesDetails(PokeRequest request);
    }
    
    public class PokeApiAdapter : IPokemonApiAdapter
    {
        private readonly HttpClient _httpClient;

        public PokeApiAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PokemonApiResponse> GetPokemon(PokeRequest request)
        {
            return await Execute<PokemonApiResponse>($"api/v2/pokemon/{request.Name}");
        }

        public async Task<SpeciesDetails> GetSpeciesDetails(PokeRequest request)
        {
            return await Execute<SpeciesDetails>($"api/v2/pokemon-species/{request.Id}");
        }

        private async Task<T> Execute<T>(string route)
        {
            try
            {
                var httpResponseMessage = await GetRetryPolicy().ExecuteAsync(async () => await _httpClient.GetAsync(route));
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                };
                
                return JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync(), jsonSerializerSettings);
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
    }
}