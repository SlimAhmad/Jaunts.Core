// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class RideAttachmentServiceException : Xeption
    {
        public RideAttachmentServiceException(Exception innerException)
            : base(message: "RideAttachment service error occurred, contact support.", innerException) { }
        public RideAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}