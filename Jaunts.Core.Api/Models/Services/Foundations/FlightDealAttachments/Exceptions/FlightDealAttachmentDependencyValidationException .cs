// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class FlightDealAttachmentDependencyValidationException : Xeption
    {
        public FlightDealAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "FlightDealAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public FlightDealAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
