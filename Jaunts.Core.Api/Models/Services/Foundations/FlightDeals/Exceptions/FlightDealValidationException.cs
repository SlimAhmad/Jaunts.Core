// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class FlightDealValidationException : Xeption
    {
        public FlightDealValidationException(Xeption innerException)
            : base(message: "FlightDeal validation error occurred, please try again.", innerException) { }
        public FlightDealValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}