// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class ShortLetAttachmentDependencyValidationException : Xeption
    {
        public ShortLetAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "ShortLetAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public ShortLetAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
