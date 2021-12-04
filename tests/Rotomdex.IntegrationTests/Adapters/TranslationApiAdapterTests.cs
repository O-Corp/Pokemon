using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Rotomdex.Integration.Adapters;

namespace Rotomdex.IntegrationTests.Adapters
{
    [TestFixture]
    public class TranslationApiAdapterTests
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient { BaseAddress = TestConfiguration.TranslatorApiSettings.BaseAddress };
        }
        
        [Test]
        public async Task When_Translating_To_Yoda_Then_Correct_Response_Is_Returned()
        {
            var subject = new YodaTranslatorAdapter(_httpClient);
            var result = await subject.Translate("hello world");
            
            Assert.That(result.Contents.Text, Is.EqualTo("hello world"));
            Assert.That(result.Contents.Translated, Is.EqualTo("Force be with you world"));
            Assert.That(result.Contents.Translation, Is.EqualTo("yoda"));
        }
        
        [Test]
        public async Task When_Translating_To_Shakespeare_Then_Correct_Response_Is_Returned()
        {
            var subject = new ShakespeareTranslatorAdapter(_httpClient);
            var result = await subject.Translate("hello world");
            
            Assert.That(result.Contents.Text, Is.EqualTo("hello world"));
            Assert.That(result.Contents.Translated, Is.EqualTo("Valorous morrow to thee,  sir ordinary"));
            Assert.That(result.Contents.Translation, Is.EqualTo("shakespeare"));
        }
    }
}