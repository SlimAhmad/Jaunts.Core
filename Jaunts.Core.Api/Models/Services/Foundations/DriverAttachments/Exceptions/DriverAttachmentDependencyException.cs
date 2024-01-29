// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class DriverAttachmentDependencyException : Xeption
    {
        public DriverAttachmentDependencyException(Exception innerException)
             : base(message: "DriverAttachment dependency error occurred, contact support.", innerException: innerException) { }
        public DriverAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
