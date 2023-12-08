// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Emails.Exceptions
{
    public class EmailProcessingDependencyValidationException : Xeption
    {
        public EmailProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Email dependency validation error occurred, please try again.",
                innerException)
        { }

        public EmailProcessingDependencyValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
