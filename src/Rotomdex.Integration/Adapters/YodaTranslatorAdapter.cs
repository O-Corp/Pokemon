using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Rotomdex.Integration.Contracts.FunTranslate.Contracts;

namespace Rotomdex.Integration.Adapters
{
    public class YodaTranslatorAdapter : ITranslationsApiAdapter
    {
        private readonly HttpClient _httpClient;

        public YodaTranslatorAdapter(HttpClient httpClient, Uri baseAddress)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
        }

        public virtual async Task<TranslationResponse> Translate(string text)
        {
            var request = new FunTranslationRequest { Text = text };
            var response = await _httpClient.PostAsJsonAsync("translate/yoda", request);
            var translationResponse = await response.Content.ReadFromJsonAsync<TranslationResponse>();
            
            return translationResponse;
        }
    }
}