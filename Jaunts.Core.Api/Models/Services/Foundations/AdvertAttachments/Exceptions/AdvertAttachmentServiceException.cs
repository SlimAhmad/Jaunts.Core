// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class AdvertAttachmentServiceException : Xeption
    {
        public AdvertAttachmentServiceException(Exception innerException)
            : base(message: "AdvertAttachment service error occurred, contact support.", innerException) { }
        public AdvertAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}