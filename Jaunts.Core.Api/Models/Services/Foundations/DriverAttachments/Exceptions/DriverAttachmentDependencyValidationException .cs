// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class DriverAttachmentDependencyValidationException : Xeption
    {
        public DriverAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "DriverAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public DriverAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
