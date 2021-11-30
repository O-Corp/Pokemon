using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rotomdex.Web.Api.Exceptions;

namespace Rotomdex.Web.Api.Middleware
{
    public class ServiceUnavailableMiddleware
    {
        private readonly RequestDelegate _next;

        public ServiceUnavailableMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ThirdPartyUnavailableException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            }
        }
    }
}