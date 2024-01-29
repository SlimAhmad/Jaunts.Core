// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class NullPackageAttachmentException : Xeption
    {
        public NullPackageAttachmentException() : base(message: "The PackageAttachment is null.") { }
        public NullPackageAttachmentException(string message) : base(message) { }
    }
}
