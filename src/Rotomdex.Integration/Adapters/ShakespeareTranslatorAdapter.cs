using System.Net.Http;

namespace Rotomdex.Integration.Adapters
{
    public class ShakespeareTranslatorAdapter : FunTranslationApiAdapter
    {
        public ShakespeareTranslatorAdapter(HttpClient httpClient) 
            : base(httpClient)
        {
        }

        protected override string TranslationType => "shakespeare";
    }
}