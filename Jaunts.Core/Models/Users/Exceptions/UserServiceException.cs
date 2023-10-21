using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class UserServiceException : Exception
    {
        public UserServiceException(Exception innerException)
            : base(message: "Service error occurred, contact support.", innerException) { }

        public UserServiceException(string message, Exception innerException)
        : base(message: message,
              innerException)
        { }
    }
}
