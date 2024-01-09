// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class ProvidersDirectorAttachmentDependencyValidationException : Xeption
    {
        public ProvidersDirectorAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "ProvidersDirectorAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public ProvidersDirectorAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
