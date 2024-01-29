// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class PackageAttachmentValidationException : Xeption
    {
        public PackageAttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public PackageAttachmentValidationException(string message,Exception innerException)
            : base(message, innerException) { }

    }
}