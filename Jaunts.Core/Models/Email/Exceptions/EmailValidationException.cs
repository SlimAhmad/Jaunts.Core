using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class EmailValidationException : Xeption
    {
        public EmailValidationException(Xeption innerException)
            : base(message: "Auth validation error occurred, fix errors and try again.",
                  innerException)
        { }

        public EmailValidationException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}