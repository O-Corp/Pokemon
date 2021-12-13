using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Builders;
using Rotomdex.Integration.Contracts.PokeApi;
using Rotomdex.Integration.Factories;
using Rotomdex.Testing.Common.Fakes;

namespace Rotomdex.Integration.UnitTests.Factories
{
    [TestFixture]
    public class TranslatorFactoryTests
    {
        [TestCase(TranslationType.Shakespeare, typeof(ShakespeareTranslatorAdapter))]
        [TestCase(TranslationType.Yoda, typeof(YodaTranslatorAdapter))]
        public void When_Creating_Instance_Of_Translator_Then_Correct_One_Is_Returned(TranslationType translationType,
            Type adapter)
        {
            var subject = new TranslatorFactory(new List<ITranslationsApiAdapter>
            {
                new ShakespeareTranslatorAdapter(new HttpClient(new ErrorHttpMessageHandler())),
                new YodaTranslatorAdapter(new HttpClient(new ErrorHttpMessageHandler()))
            });

            var result = subject.Create(translationType);
            Assert.That(result, Is.TypeOf(adapter));
        }
    }

    [TestFixture]
    public class PokemonApiResponseBuilderTests
    {
        private PokemonApiResponse _result;

        [SetUp]
        public void Setup()
        {
            _result = new PokemonApiResponseBuilder()
                .WithId(123)
                .WithName("Rashiram")
                .WithHabitat("Rare")
                .WithIsLegendary(true)
                .WithDescriptions(new List<Description>()
                {
                    new() { Language = new Language { Name = "en" }, FlavourText = "Arsene Wenger" },
                    new() { Language = new Language { Name = "fr" }, FlavourText = "Thierry Henry" }                    
                })
                .Build();
        }

        [Test]
        public void When_Building_Response_Then_Id_Is_Mapped_Correctly()
        {
            Assert.That(_result.Id, Is.EqualTo(123));
        }
        
        [Test]
        public void When_Building_Response_Then_Name_Is_Mapped_Correctly()
        {
            Assert.That(_result.Name, Is.EqualTo("Rashiram"));
        }
        
        [Test]
        public void When_Building_Response_Then_Is_Legendary_Is_Mapped_Correctly()
        {
            Assert.That(_result.IsLegendary, Is.True);
        }
        
        [Test]
        public void When_Building_Response_Then_Habitat_Is_Mapped_Correctly()
        {
            Assert.That(_result.Habitat, Is.EqualTo("Rare"));
        }
        
        [Test]
        public void When_Building_Response_Then_Descriptions_Are_Mapped_Correctly()
        {
            Assert.That(_result.Descriptions[0].FlavourText, Is.EqualTo("Arsene Wenger"));
            Assert.That(_result.Descriptions[0].Language.Name, Is.EqualTo("en"));
            
            Assert.That(_result.Descriptions[1].FlavourText, Is.EqualTo("Thierry Henry"));
            Assert.That(_result.Descriptions[1].Language.Name, Is.EqualTo("fr"));
        }
    }
}