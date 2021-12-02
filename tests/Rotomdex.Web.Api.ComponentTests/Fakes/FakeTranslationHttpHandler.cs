using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rotomdex.Web.Api.ComponentTests.Fakes
{
    public class FakeTranslationHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}