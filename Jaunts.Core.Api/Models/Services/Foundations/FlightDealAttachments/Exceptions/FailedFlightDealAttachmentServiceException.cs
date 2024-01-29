// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class FailedFlightDealAttachmentServiceException : Xeption
    {
        public FailedFlightDealAttachmentServiceException(Exception innerException)
            : base(message: "Failed FlightDealAttachment service error occurred, please contact support.",
                  innerException)
        { }
        public FailedFlightDealAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
