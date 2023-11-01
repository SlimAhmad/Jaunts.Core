using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class FailedEmailServiceException : Xeption
    {
        public FailedEmailServiceException(Exception innerException)
            : base(message: "Failed Auth service error occurred, contact support.",
                  innerException)
        { }

        public FailedEmailServiceException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}