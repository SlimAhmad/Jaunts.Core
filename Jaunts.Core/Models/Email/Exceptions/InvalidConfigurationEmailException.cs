using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class InvalidConfigurationEmailException : Xeption
    {
        public InvalidConfigurationEmailException(Exception innerException)
            : base(message: "Invalid Auth configuration error occurred, contact support.",
                  innerException)
        { }

        public InvalidConfigurationEmailException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}