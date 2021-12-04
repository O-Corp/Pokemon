using System.Net.Http;

namespace Rotomdex.Integration.Adapters
{
    public class YodaTranslatorAdapter : FunTranslationApiAdapter
    {
        public YodaTranslatorAdapter(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string TranslationVersion => "yoda";
    }
}