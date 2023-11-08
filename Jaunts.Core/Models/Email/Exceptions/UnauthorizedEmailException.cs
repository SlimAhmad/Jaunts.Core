using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class UnauthorizedEmailException : Xeption
    {
        public UnauthorizedEmailException(Exception innerException)
            : base(message: "Unauthorized Email request, fix errors and try again.",
                  innerException)
        { }

        public UnauthorizedEmailException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}