using NUnit.Framework;
using Rotomdex.Domain.Exceptions;
using Rotomdex.Domain.Models;

namespace Rotomdex.Domain.UnitTests.Models
{
    [TestFixture]
    public class PokemonTests
    {
        [Test]
        public void When_Creating_A_Pokemon_Then_It_Is_Created_Successfully()
        {
            var subject = Pokemon.Create(
                "mewtwo",
                "description",
                "rare",
                true);

            Assert.That(subject.Name, Is.EqualTo("Mewtwo"));
            Assert.That(subject.Description, Is.EqualTo("description"));
            Assert.That(subject.Habitat, Is.EqualTo("Rare"));
            Assert.That(subject.IsLegendary, Is.True);
            Assert.That(subject.ToString(), Is.EqualTo("Mewtwo"));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void When_Creating_Pokemon_With_Invalid_Name_Then_Throw_Exception(string name)
        {
             var exception = Assert.Throws<InvalidPokemonException>(() => Pokemon.Create(name, "testing", "rare", true));
             Assert.That(exception?.Message, Is.EqualTo($"The value {name} is invalid for field Name."));
        }
    }
}