using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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

            return Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(_translatedText),
                StatusCode = HttpStatusCode.OK
            });
        }
        
        public HttpRequestMessage HttpRequest { get; private set; }
    }
}