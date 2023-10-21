using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class UserDependencyException : Exception
    {
        public UserDependencyException(Exception innerException)
            : base(message: "Service dependency error occurred, contact support.", innerException) { }

        public UserDependencyException(string message, Exception innerException)
        : base(message: message,
              innerException)
        { }
    }
}
