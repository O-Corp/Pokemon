using System.Threading.Tasks;
using Rotomdex.Domain.Models;

namespace Rotomdex.Integration.Decorators.Translation
{
    public interface ITranslationDecorator
    {
        Task<Domain.Models.Translation> Translate(Pokemon pokemon);
    }
}