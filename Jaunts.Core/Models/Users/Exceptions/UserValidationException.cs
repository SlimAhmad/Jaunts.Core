using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class UserValidationException : Exception
    {
        public UserValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }

        public UserValidationException(string message, Exception innerException)
        : base(message: message,
              innerException)
        { }
    }
}
