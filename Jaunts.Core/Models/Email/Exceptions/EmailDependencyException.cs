using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public class EmailDependencyException : Xeption
    {
        public EmailDependencyException(Xeption innerException)
            : base(message: "Auth dependency error occurred, contact support.",
                  innerException)
        { }

        public EmailDependencyException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}