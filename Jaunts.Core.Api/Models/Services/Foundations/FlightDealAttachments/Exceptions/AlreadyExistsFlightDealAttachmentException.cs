// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class AlreadyExistsFlightDealAttachmentException : Xeption
    {
        public AlreadyExistsFlightDealAttachmentException(Exception innerException)
            : base(message: "FlightDealAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsFlightDealAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
