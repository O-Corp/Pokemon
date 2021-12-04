using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rotomdex.Testing.Common.Fakes
{
    public class FakeYodaTranslationHttpMessageHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage? request, CancellationToken cancellationToken)
        {
            ExecutedRequest = request;
            
            var json = await File.ReadAllTextAsync("Data/yoda_translation.json", cancellationToken);
            return new HttpResponseMessage
            {
                Content = new StringContent(json),
                StatusCode = HttpStatusCode.OK
            };
        }

        public static HttpRequestMessage? ExecutedRequest { get; private set; }
    }
}