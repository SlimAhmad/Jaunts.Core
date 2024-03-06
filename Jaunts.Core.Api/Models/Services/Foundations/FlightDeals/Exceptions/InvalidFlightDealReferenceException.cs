// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class InvalidFlightDealReferenceException : Xeption
    {
        public InvalidFlightDealReferenceException(Exception innerException)
            : base(message: "Invalid flightDeal reference error occurred.", innerException)
        { }
        public InvalidFlightDealReferenceException(string message)
            : base(message)
        { }
    }
}
