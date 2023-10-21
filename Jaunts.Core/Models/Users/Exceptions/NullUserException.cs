using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class NullUserException : Exception
    {
        public NullUserException() : base(message: "The user is null.") { }

        public NullUserException(string message, Exception innerException)
        : base(message: message,
              innerException)
        { }
    }
}
