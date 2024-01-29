// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class AdvertAttachmentDependencyException : Xeption
    {
        public AdvertAttachmentDependencyException(Exception innerException)
             : base(message: "AdvertAttachment dependency error occurred, contact support.", innerException: innerException) { }
        public AdvertAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
