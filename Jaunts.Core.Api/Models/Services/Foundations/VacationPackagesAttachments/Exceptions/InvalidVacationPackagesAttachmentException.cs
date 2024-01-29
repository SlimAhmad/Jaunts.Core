// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class InvalidPackageAttachmentException : Xeption
    {
        public InvalidPackageAttachmentException()
         : base(message: $"Invalid PackageAttachment. Please correct the errors and try again.")
        { }

        public InvalidPackageAttachmentException(string message)
            : base(message)
        { }
    }
}