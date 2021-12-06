using System;

namespace Rotomdex.Domain.Exceptions
{
    public class InvalidPokemonException : Exception
    {
        public InvalidPokemonException(string field, string value)
            : base($"The value {value} is invalid for field {field}.")
        {
        }
    }
}