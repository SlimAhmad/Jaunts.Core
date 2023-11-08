using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class EmailDependencyValidationException : Xeption
    {
        public EmailDependencyValidationException(Xeption innerException)
            : base(message: "Email dependency validation error occurred, contact support.",
                  innerException)
        { }

        public EmailDependencyValidationException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}