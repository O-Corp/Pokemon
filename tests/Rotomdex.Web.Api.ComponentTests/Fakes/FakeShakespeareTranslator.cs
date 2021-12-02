using System;
using System.Net.Http;
using System.Threading.Tasks;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Contracts.FunTranslate.Contracts;

namespace Rotomdex.Web.Api.ComponentTests.Fakes
{
    public class FakeShakespeareTranslator : ShakespeareTranslatorAdapter
    {
        public FakeShakespeareTranslator()
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
                    Translated = "Give every man thy ear, but few thy voice"
                }
            });
        }
        
        public string Text { get; private set; }
    }
}