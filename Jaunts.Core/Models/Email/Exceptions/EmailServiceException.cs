using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class EmailServiceException : Xeption
    {
        public EmailServiceException(Xeption innerException)
            : base(message: "Auth service error occurred, contact support.",
                  innerException)
        { }

        public EmailServiceException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}