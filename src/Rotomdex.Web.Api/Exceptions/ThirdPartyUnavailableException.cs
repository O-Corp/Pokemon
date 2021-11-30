using System;

namespace Rotomdex.Web.Api.Exceptions
{
    public class ThirdPartyUnavailableException : Exception
    {
        public ThirdPartyUnavailableException(string thirdPartyName, Exception exception) 
            : base($"Unable to connect to service {thirdPartyName}. Message {exception.Message}. Details {exception.StackTrace}.")
        {
        }
    }
}