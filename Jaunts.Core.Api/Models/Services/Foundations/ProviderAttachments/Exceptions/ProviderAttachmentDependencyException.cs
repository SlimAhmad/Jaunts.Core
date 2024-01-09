// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class ProviderAttachmentDependencyException : Xeption
    {
        public ProviderAttachmentDependencyException(Exception innerException)
             : base(message: "ProviderAttachment dependency error occurred, contact support.", innerException) { }
        public ProviderAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
