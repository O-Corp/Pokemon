using System.Threading.Tasks;
using Rotomdex.Domain.Models;

namespace Rotomdex.Integration.Decorators
{
    public interface ITranslationDecorator
    {
        Task<Translation> Translate(Pokemon pokemon);
    }
}