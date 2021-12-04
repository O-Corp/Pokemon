using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rotomdex.Testing.Common.Fakes
{
    public class ErrorHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new TaskCanceledException();
        }
    }
}