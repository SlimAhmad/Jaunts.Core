using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class InvalidEmailException : Xeption
    {
        public InvalidEmailException()
            : base(message: "Invalid Email error occurred, fix errors and try again.")
        { }

        public InvalidEmailException(Exception innerException)
            : base(message: "Invalid Email error occurred, fix errors and try again.",
                  innerException)
        { }

        public InvalidEmailException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}