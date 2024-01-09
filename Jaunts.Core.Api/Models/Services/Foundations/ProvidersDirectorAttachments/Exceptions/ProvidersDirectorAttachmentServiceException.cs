// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class ProvidersDirectorAttachmentServiceException : Xeption
    {
        public ProvidersDirectorAttachmentServiceException(Exception innerException)
            : base(message: "ProvidersDirectorAttachment service error occurred, contact support.", innerException) { }
        public ProvidersDirectorAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}