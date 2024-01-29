// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class PackageAttachmentServiceException : Xeption
    {
        public PackageAttachmentServiceException(Exception innerException)
            : base(message: "PackageAttachment service error occurred, contact support.", innerException) { }
        public PackageAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}