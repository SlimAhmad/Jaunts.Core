using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string parameterName, object parameterValue)
            : base(message: $"Invalid user, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }


    }
}
