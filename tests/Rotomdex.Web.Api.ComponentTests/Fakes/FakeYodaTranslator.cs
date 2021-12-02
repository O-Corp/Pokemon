using System;
using System.Net.Http;
using System.Threading.Tasks;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Contracts.FunTranslate.Contracts;

namespace Rotomdex.Web.Api.ComponentTests.Fakes
{
    public class FakeYodaTranslator : YodaTranslatorAdapter
    {
        public FakeYodaTranslator()
            : base(new HttpClient(new FakeTranslationHttpHandler()), new Uri("http://foo.com/"))
        {
        }

        public override Task<TranslationResponse> Translate(string text)
        {
            Text = text;
            
            return Task.FromResult(new TranslationResponse
            {
                Contents = new Contents
                {
                    Translated = "Fear is the path to the dark side"
                }
            });
        }
        
        public string Text { get; private set; }
    }
}