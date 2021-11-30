using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Rotomdex.Web.Api.UnitTests.Adapters
{
    internal class FakeHttpHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, TestExpectation> _responses;

        public FakeHttpHandler()
        {
            _responses = new Dictionary<string, TestExpectation>();
            HttpRequests = new List<HttpRequestMessage>();
        }

        public void SetupResponse(string uri, string jsonResponse, HttpStatusCode httpStatusCode)
        {
            _responses.Add(uri, new TestExpectation(jsonResponse, httpStatusCode));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequests.Add(request);
            
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            if (_responses.TryGetValue(request.RequestUri.ToString(), out var expectation))
            {
                if (!string.IsNullOrWhiteSpace(expectation.Json))
                {
                    httpResponseMessage.Content = new StringContent(expectation.Json);    
                }
                
                httpResponseMessage.StatusCode = expectation.HttpStatusCode;
            }

            return Task.FromResult(httpResponseMessage);
        }

        public List<HttpRequestMessage> HttpRequests { get; }
    }
}