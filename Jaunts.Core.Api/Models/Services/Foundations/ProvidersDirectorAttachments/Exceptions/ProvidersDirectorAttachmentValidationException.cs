// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class ProvidersDirectorAttachmentValidationException : Xeption
    {
        public ProvidersDirectorAttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public ProvidersDirectorAttachmentValidationException(string message,Exception innerException)
            : base(message, innerException) { }

    }
}