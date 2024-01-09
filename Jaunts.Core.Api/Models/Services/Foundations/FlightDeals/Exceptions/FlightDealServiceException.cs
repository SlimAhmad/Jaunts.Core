// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class FlightDealServiceException : Xeption
    {
        public FlightDealServiceException(Xeption innerException)
            : base(message: "FlightDeal service error occurred, contact support.", innerException) { }
        public FlightDealServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}