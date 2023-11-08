using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class EmailServiceException : Xeption
    {
        public EmailServiceException(Xeption innerException)
            : base(message: "Email service error occurred, contact support.",
                  innerException)
        { }

        public EmailServiceException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}