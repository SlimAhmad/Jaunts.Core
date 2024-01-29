// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class FlightDealAttachmentServiceException : Xeption
    {
        public FlightDealAttachmentServiceException(Exception innerException)
            : base(message: "FlightDealAttachment service error occurred, contact support.", innerException) { }
        public FlightDealAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}