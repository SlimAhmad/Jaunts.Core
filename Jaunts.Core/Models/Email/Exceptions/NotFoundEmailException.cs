using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class NotFoundEmailException : Xeption
    {
        public NotFoundEmailException(Exception innerException)
            : base(message: "Not found Email error occurred, fix errors and try again.",
                  innerException)
        { }

        public NotFoundEmailException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}