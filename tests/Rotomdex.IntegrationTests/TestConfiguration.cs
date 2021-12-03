using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Rotomdex.Web.Api.Configuration;

namespace Rotomdex.IntegrationTests
{
    [SetUpFixture]
    public class TestConfiguration
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
            
            PokeApiSettings = new PokeApiSettings();
            config.Bind(nameof(Web.Api.Configuration.PokeApiSettings), PokeApiSettings);
            
            TranslatorApiSettings = new TranslatorApiSettings();
            config.Bind(nameof(TranslatorApiSettings), TranslatorApiSettings);
        }
        
        public static PokeApiSettings PokeApiSettings { get; private set; }
        
        public static TranslatorApiSettings TranslatorApiSettings { get; private set; }
    }
}