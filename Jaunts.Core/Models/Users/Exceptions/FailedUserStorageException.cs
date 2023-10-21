using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class FailedUserStorageException : Xeption
    {
        public FailedUserStorageException(Exception innerException)
            : base(message: "Failed user storage error occurred, contact support.",
                  innerException)
        { }

        public FailedUserStorageException(string message, Exception innerException)
        : base(message: message,
              innerException)
        { }
    }
}
