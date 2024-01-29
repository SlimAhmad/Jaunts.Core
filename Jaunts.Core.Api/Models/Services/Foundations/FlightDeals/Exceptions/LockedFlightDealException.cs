// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class LockedFlightDealException : Xeption
    {
        public LockedFlightDealException(Exception innerException)
            : base(message: "Locked flightDeal record exception, please try again later.", innerException) { }
        public LockedFlightDealException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
