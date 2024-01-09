// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class ProvidersDirectorAttachmentDependencyException : Xeption
    {
        public ProvidersDirectorAttachmentDependencyException(Exception innerException)
             : base(message: "ProvidersDirectorAttachment dependency error occurred, contact support.", innerException) { }
        public ProvidersDirectorAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
