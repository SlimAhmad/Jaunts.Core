using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class ExcessiveCallEmailException : Xeption
    {
        public ExcessiveCallEmailException(Exception innerException)
            : base(message: "Excessive call error occurred, limit your calls.",
                  innerException)
        { }

        public ExcessiveCallEmailException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}