// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class RideAttachmentDependencyException : Xeption
    {
        public RideAttachmentDependencyException(Exception innerException)
             : base(message: "RideAttachment dependency error occurred, contact support.", innerException) { }
        public RideAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
