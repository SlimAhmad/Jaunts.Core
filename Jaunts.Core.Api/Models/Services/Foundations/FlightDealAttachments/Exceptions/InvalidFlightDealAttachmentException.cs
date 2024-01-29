// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class InvalidFlightDealAttachmentException : Xeption
    {
        public InvalidFlightDealAttachmentException()
           : base(message: "Invalid FlightDealAttachment. Please correct the errors and try again.")
        { }

        public InvalidFlightDealAttachmentException(string message)
            : base(message)
        { }
    }
}