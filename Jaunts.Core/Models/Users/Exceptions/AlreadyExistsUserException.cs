using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class AlreadyExistsUserException : Exception
    {
        public AlreadyExistsUserException(Exception innerException)
            : base(message: "User with the same id already exists.", innerException) { }

        public AlreadyExistsUserException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }

    }
}
