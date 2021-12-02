using System;
using System.Net.Http;

namespace Rotomdex.Integration.Adapters
{
    public class YodaTranslatorAdapter : FunTranslationApiAdapter
    {
        public YodaTranslatorAdapter(HttpClient httpClient, Uri baseAddress) : base(httpClient, baseAddress)
        {
        }

        protected override string TranslationVersion => "yoda";
    }
}