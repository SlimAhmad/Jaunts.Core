// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class FailedFlightDealServiceException : Xeption
    {
        public FailedFlightDealServiceException(Exception innerException)
            : base(message: "Failed flightDeal service error occurred, contact support.",
                  innerException)
        { }
        public FailedFlightDealServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
