// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class NullProvidersDirectorAttachmentException : Xeption
    {
        public NullProvidersDirectorAttachmentException() : base(message: "The ProvidersDirectorAttachment is null.") { }
        public NullProvidersDirectorAttachmentException(string message) : base(message) { }
    }
}
