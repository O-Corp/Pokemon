using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rotomdex.Domain.DTOs;
using Rotomdex.Integration.Contracts.PokeApi;

namespace Rotomdex.Testing.Common.Fakes
{
    public class FakePokeApiHttpHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, TestExpectation> _responses;

        public FakePokeApiHttpHandler()
        {
            _responses = new Dictionary<string, TestExpectation>();
            HttpRequests = new List<HttpRequestMessage>();
        }

        public void SetupPokemonResponse(PokeRequest request, PokeInfoResponse response)
        {
            var json = JsonConvert.SerializeObject(response);
            _responses.Add($"/api/v2/pokemon/{request.Name.ToLower()}", new TestExpectation(json, HttpStatusCode.OK));
        }
        
        public void SetupSpeciesResponse(PokeRequest request, SpeciesDetails response)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
            var json = JsonConvert.SerializeObject(response, serializerSettings);
            _responses.Add($"/api/v2/pokemon-species/{request.Id}", new TestExpectation(json, HttpStatusCode.OK));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequests.Add(request);
            
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            if (request.RequestUri is null)
            {
                return Task.FromResult(httpResponseMessage);
            }
            
            if (!_responses.TryGetValue(request.RequestUri.PathAndQuery, out var expectation))
            {
                return Task.FromResult(httpResponseMessage);
            }
                
            if (!string.IsNullOrWhiteSpace(expectation.Json))
            {
                httpResponseMessage.Content = new StringContent(expectation.Json);    
            }
                
            httpResponseMessage.StatusCode = expectation.HttpStatusCode;

            return Task.FromResult(httpResponseMessage);
        }

        public List<HttpRequestMessage> HttpRequests { get; }
    }
}