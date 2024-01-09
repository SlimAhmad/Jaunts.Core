// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class ProviderAttachmentDependencyValidationException : Xeption
    {
        public ProviderAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "ProviderAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public ProviderAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
