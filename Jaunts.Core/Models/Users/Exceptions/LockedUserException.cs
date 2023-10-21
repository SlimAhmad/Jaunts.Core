using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class LockedUserException : Exception
    {
        public LockedUserException(Exception innerException)
            : base(message: "Locked user record exception, please try again later.", innerException) { }

        public LockedUserException(string message, Exception innerException)
        : base(message: message,
              innerException)
        { }
    }
}
