using System.Net;

namespace Rotomdex.Testing.Common.Fakes
{
    public class TestExpectation
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