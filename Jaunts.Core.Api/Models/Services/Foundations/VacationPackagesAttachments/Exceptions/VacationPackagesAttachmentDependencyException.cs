// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class PackageAttachmentDependencyException : Xeption
    {
        public PackageAttachmentDependencyException(Exception innerException)
             : base(message: "PackageAttachment dependency error occurred, contact support.", innerException) { }
        public PackageAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
