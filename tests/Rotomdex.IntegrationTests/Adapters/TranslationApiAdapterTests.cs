using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Rotomdex.Integration.Adapters;
using Rotomdex.Web.Api.Configuration;

namespace Rotomdex.IntegrationTests.Adapters
{
    [TestFixture]
    public class TranslationApiAdapterTests
    {
        private TranslatorApiSettings _translatorApiSettings;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
            
            _translatorApiSettings = new TranslatorApiSettings();
            config.Bind(nameof(TranslatorApiSettings), _translatorApiSettings);
        }
        
        [Test]
        public async Task When_Translating_To_Yoda_Then_Correct_Response_Is_Returned()
        {
            var subject = new YodaTranslatorAdapter(new HttpClient(), _translatorApiSettings.BaseAddress);
            var result = await subject.Translate("hello world");
            
            Assert.That(result.Contents.Text, Is.EqualTo("hello world"));
            Assert.That(result.Contents.Translated, Is.EqualTo("Force be with you world"));
            Assert.That(result.Contents.Translation, Is.EqualTo("yoda"));
        }
        
        [Test]
        public async Task When_Translating_To_Shakespeare_Then_Correct_Response_Is_Returned()
        {
            var subject = new ShakespeareTranslatorAdapter(new HttpClient(), _translatorApiSettings.BaseAddress);
            var result = await subject.Translate("hello world");
            
            Assert.That(result.Contents.Text, Is.EqualTo("hello world"));
            Assert.That(result.Contents.Translated, Is.EqualTo("Valorous morrow to thee,  sir ordinary"));
            Assert.That(result.Contents.Translation, Is.EqualTo("shakespeare"));
        }
    }
}