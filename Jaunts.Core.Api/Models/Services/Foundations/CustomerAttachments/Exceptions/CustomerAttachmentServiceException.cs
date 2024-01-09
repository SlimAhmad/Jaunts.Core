// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class CustomerAttachmentServiceException : Xeption
    {
        public CustomerAttachmentServiceException(Exception innerException)
            : base(message: "CustomerAttachment service error occurred, contact support.", innerException) { }
        public CustomerAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}