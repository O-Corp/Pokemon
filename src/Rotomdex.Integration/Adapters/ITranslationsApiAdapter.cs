using System.Threading.Tasks;

namespace Rotomdex.Integration.Adapters
{
    public interface ITranslationsApiAdapter
    {
        Task<string> Translate(string text);
    }
}