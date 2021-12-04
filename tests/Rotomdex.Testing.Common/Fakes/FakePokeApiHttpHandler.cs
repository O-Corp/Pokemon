using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rotomdex.Testing.Common.Fakes
{
    public class PokeApiResponses
    {
        public const string PokeApi_Details_Response = "Data/pokemon_details.json";
        public const string PokeApi_Species_Response = "Data/pokemon_species.json";
    }
    
    public class FakePokeApiHttpHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, TestExpectation> _responses;

        public FakePokeApiHttpHandler()
        {
            _responses = new Dictionary<string, TestExpectation>();
            HttpRequests = new List<HttpRequestMessage>();
        }
        
        public async Task SetupResponse(string route, string jsonFile, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            var json = await File.ReadAllTextAsync(jsonFile);
            _responses.Add(route, new TestExpectation(json, httpStatusCode));
        }

        public void SetupResponse(string route, HttpStatusCode httpStatusCode)
        {
            _responses.Add(route, new TestExpectation(string.Empty, httpStatusCode));
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
    
    public class TestExpectation
    {
        public TestExpectation(string json, HttpStatusCode httpStatusCode)
        {
            Json = json;
            HttpStatusCode = httpStatusCode;
        }
        
        public string Json { get; }
        
        public HttpStatusCode HttpStatusCode { get; }
    }
}