using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Rotomdex.Integration.Contracts.FunTranslate.Contracts;

namespace Rotomdex.Integration.Adapters
{
    public abstract class FunTranslationApiAdapter : ITranslationsApiAdapter
    {
        private readonly HttpClient _httpClient;

        public FunTranslationApiAdapter(HttpClient httpClient, Uri baseAddress)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
        }
        
        public async Task<TranslationResponse> Translate(string text)
        {
            var request = new FunTranslationRequest { Text = text };
            var response = await _httpClient.PostAsJsonAsync($"translate/{TranslationVersion.ToLower()}", request);
            var translationResponse = await response.Content.ReadFromJsonAsync<TranslationResponse>();
            return translationResponse;
        }
        
        protected abstract string TranslationVersion { get; }
    }
}