using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Rotomdex.Integration.Contracts.FunTranslate;

namespace Rotomdex.Integration.Adapters
{
    public interface ITranslationsApiAdapter
    {
        Task<TranslationResponse> Translate(string text);
    }
    
    public abstract class FunTranslationApiAdapter : ITranslationsApiAdapter
    {
        private readonly HttpClient _httpClient;

        protected FunTranslationApiAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<TranslationResponse> Translate(string text)
        {
            try
            {
                var request = new FunTranslationRequest { Text = text };
                var response = await _httpClient.PostAsJsonAsync($"translate/{TranslationType.ToLower()}", request);
                var translationResponse = await response.Content.ReadFromJsonAsync<TranslationResponse>();
                return translationResponse;
            }
            catch (Exception)
            {
                // TODO: log and track
            }

            return null;
        }
        
        protected abstract string TranslationType { get; }
    }
}