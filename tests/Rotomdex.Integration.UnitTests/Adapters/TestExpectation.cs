using System.Net;

namespace Rotomdex.Integration.UnitTests.Adapters
{
    internal class TestExpectation
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