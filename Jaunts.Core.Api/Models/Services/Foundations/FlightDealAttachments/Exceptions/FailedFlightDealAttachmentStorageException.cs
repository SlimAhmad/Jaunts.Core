// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class FailedFlightDealAttachmentStorageException : Xeption
    {
        public FailedFlightDealAttachmentStorageException(Exception innerException)
            : base(message: "Failed FlightDealAttachment storage error occurred, please contact support.", innerException)
        { }
        public FailedFlightDealAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
