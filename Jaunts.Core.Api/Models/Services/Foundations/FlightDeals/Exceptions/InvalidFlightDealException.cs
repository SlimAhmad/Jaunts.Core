﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class InvalidFlightDealException : Xeption
    {
        public InvalidFlightDealException(string parameterName, object parameterValue)
            : base(message: $"Invalid flightDeal, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }
        public InvalidFlightDealException()
            : base(message: "Invalid flightDeal. Please fix the errors and try again.")
        { }
        public InvalidFlightDealException(string message)
            : base(message)
        { }
    }
}