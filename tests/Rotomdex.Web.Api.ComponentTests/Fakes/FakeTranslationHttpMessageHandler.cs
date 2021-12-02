using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Rotomdex.Integration.Contracts.FunTranslate.Contracts;

namespace Rotomdex.Web.Api.ComponentTests.Fakes
{
    public class FakeTranslationHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _translatedText;

        public FakeTranslationHttpMessageHandler(string translatedText)
        {
            _translatedText = translatedText;
        }
            
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequest = request;
            
            var httpResponseMessage = new HttpResponseMessage();
            var translationResponse = new TranslationResponse
            {
                Contents = new Contents
                {
                    Translated = _translatedText
                }
            };
            httpResponseMessage.Content = new ObjectContent<TranslationResponse>(translationResponse, new JsonMediaTypeFormatter());
            return Task.FromResult(httpResponseMessage);
        }
        
        public HttpRequestMessage HttpRequest { get; private set; }
    }
}