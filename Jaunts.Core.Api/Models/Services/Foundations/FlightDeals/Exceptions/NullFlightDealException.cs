// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class NullFlightDealException : Xeption
    {
        public NullFlightDealException() : base(message: "The flightDeal is null.") { }
        public NullFlightDealException(string message) : base(message) { }
    }
}
