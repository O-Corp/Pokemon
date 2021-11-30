using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rotomdex.Integration.UnitTests.Adapters
{
    internal class ErrorHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new TaskCanceledException();
        }
    }
}