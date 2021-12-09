using NUnit.Framework;
using Rotomdex.Domain.Models;

namespace Rotomdex.Domain.UnitTests.Models
{
    [TestFixture]
    public class TranslationTests
    {
        [Test]
        public void When_Creating_Translation_Then_It_Is_Created_Successfully()
        {
            var subject = new Translation("hello world");
            Assert.That(subject.ToString(), Is.EqualTo("hello world"));
        }
    }
}