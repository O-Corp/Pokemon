using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Rotomdex.Web.Api.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Rotomdex.Web.Api.ComponentTests.Steps
{
    [Binding]
    [Scope(Feature = "Translation")]
    public class TranslationSteps
    {
        private DataContainer _dataContainer;
        private HttpResponseMessage _httpResponse;
        private TranslationRequest _payload;

        [Before]
        public void Setup()
        {
            _dataContainer = new DataContainer();
        }
        
        [Given(@"a request to translate (.*)")]
        public void GivenARequestToTranslate(string name)
        {
            _payload = new TranslationRequest
            {
                Name = name
            };
        }
        
        [When(@"the request is sent")]
        public async Task WhenTheRequestIsSent()
        {
            using (var client = TestHelper.CreateHttpClient(_dataContainer))
            {
                _httpResponse = await client.PostAsJsonAsync($"http://localhost/rotomdex/v1/pokemon/translate", _payload);
            }
        }

        [Then(@"an (.*) response is returned")]
        public void ThenAnResponseIsReturned(HttpStatusCode httpStatusCode)
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Then(@"the response is translated with")]
        public async Task ThenTheResponseIsTranslatedWith(Table table)
        {
            var expected = table.CreateInstance<PokemonResponse>();
            var result = await _httpResponse.Content.ReadAsAsync<PokemonResponse>();

            Assert.That(result.Habitat, Is.EqualTo(expected.Habitat));
            Assert.That(result.Name, Is.EqualTo(expected.Name));
            Assert.That(result.DescriptionStandard, Is.EqualTo(expected.DescriptionStandard));
            Assert.That(result.IsLegendary, Is.EqualTo(expected.IsLegendary));        }
    }
}