// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class CustomerAttachmentDependencyException : Xeption
    {
        public CustomerAttachmentDependencyException(Exception innerException)
             : base(message: "CustomerAttachment dependency error occurred, contact support.", innerException) { }
        public CustomerAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
