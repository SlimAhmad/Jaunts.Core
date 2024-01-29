// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class ShortLetAttachmentServiceException : Xeption
    {
        public ShortLetAttachmentServiceException(Exception innerException)
            : base(message: "ShortLetAttachment service error occurred, contact support.", innerException) { }
        public ShortLetAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}