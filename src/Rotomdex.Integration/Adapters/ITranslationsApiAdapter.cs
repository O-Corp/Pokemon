using System.Threading.Tasks;
using Rotomdex.Integration.Contracts.FunTranslate.Contracts;

namespace Rotomdex.Integration.Adapters
{
    public interface ITranslationsApiAdapter
    {
        Task<TranslationResponse> Translate(string text);
    }
}