// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Emails.Exceptions
{
    public class EmailProcessingValidationException : Xeption
    {
        public EmailProcessingValidationException(Xeption innerException)
            : base(message: "Email validation error occurred, please try again.", innerException)
        { }

        public EmailProcessingValidationException(string message,Xeption innerException)
           : base(message, innerException)
        { }
    }
}
