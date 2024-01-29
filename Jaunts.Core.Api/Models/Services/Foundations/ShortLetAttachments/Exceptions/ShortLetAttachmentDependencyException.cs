// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class ShortLetAttachmentDependencyException : Xeption
    {
        public ShortLetAttachmentDependencyException(Exception innerException)
             : base(message: "ShortLetAttachment dependency error occurred, contact support.", innerException) { }
        public ShortLetAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
