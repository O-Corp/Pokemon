using System;
using System.Net.Http;
using Rotomdex.Testing.Common.Fakes;

namespace Rotomdex.Testing.Common.Helpers
{
    public class HttpClientBuilder
    {
        private readonly Uri _baseAddress;
        private HttpMessageHandler _handler;

        public HttpClientBuilder(Uri baseAddress)
        {
            _baseAddress = baseAddress;
            _handler = new HttpClientHandler();
        }
        
        public HttpClientBuilder WithYodaTranslation()
        {
            _handler = new FakeYodaTranslationHttpMessageHandler();
            return this;
        }

        public HttpClientBuilder WithShakespeareTranslation()
        {
            _handler = new FakeShakespeareTranslationHttpMessageHandler();
            return this;
        }

        public HttpClientBuilder WithUnavailableApi()
        {
            _handler = new ErrorHttpMessageHandler();
            return this;
        }

        public HttpClientBuilder WithPokeApi()
        {
            var fakePokeApiHttpHandler = new FakePokeApiHttpHandler();
            //fakePokeApiHttpHandler.SetupResponse()
            _handler = fakePokeApiHttpHandler;
            return this;
        }
        
        public HttpClient Build()
        {
            return new HttpClient(_handler)
            {
                BaseAddress = _baseAddress
            };
        }
    }
}