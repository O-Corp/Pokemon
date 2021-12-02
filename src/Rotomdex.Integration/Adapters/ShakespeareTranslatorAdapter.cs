using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Rotomdex.Integration.Contracts.FunTranslate.Contracts;

namespace Rotomdex.Integration.Adapters
{
    public class ShakespeareTranslatorAdapter : FunTranslationApiAdapter
    {
        public ShakespeareTranslatorAdapter(HttpClient httpClient, Uri baseAddress) : base(httpClient, baseAddress)
        {
        }

        protected override string TranslationVersion => "shakespeare";
    }
}