using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class FailedUserServiceException : Xeption
    {
        public FailedUserServiceException(Exception innerException)
            : base(message: "Failed user service error occurred.", innerException)
        { }

        public FailedUserServiceException(string message, Exception innerException)
        : base(message: message,
              innerException)
        { }
    }
}
