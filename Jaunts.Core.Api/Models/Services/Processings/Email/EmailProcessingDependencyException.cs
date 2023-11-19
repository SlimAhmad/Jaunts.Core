// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Emails.Exceptions
{
    public class EmailProcessingDependencyException : Xeption
    {
        public EmailProcessingDependencyException(Xeption innerException)
            : base(message: "Email Impression dependency error occurred, please contact support", innerException)
        { }
    }
}
