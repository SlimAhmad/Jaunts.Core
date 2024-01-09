// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class NotFoundFlightDealException : Xeption
    {
        public NotFoundFlightDealException(Guid flightDealId)
            : base(message: $"Couldn't find FlightDeal with id: {flightDealId}.") { }
        public NotFoundFlightDealException(string message)
            : base(message) { }
    }
}
