// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class FlightDealAttachmentValidationException : Xeption
    {
        public FlightDealAttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public FlightDealAttachmentValidationException(string message,Exception innerException)
            : base(message, innerException) { }

    }
}