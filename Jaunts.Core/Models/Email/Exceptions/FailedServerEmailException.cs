using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class FailedServerEmailException : Xeption
    {
        public FailedServerEmailException(Exception innerException)
            : base(message: "Failed Email server error occurred, contact support.",
                  innerException)
        { }

        public FailedServerEmailException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}