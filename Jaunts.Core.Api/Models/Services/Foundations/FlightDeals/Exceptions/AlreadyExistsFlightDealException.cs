// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class AlreadyExistsFlightDealException : Xeption
    {
        public AlreadyExistsFlightDealException(Exception innerException)
            : base(message: "FlightDeal with the same id already exists.", innerException) { }
        public AlreadyExistsFlightDealException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
