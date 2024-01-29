// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class PackageAttachmentDependencyValidationException : Xeption
    {
        public PackageAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "PackageAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public PackageAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
