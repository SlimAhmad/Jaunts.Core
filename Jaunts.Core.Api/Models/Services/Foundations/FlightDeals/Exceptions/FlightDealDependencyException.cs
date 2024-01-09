// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class FlightDealDependencyException : Xeption
    {
        public FlightDealDependencyException(Xeption innerException)
             : base(message: "FlightDeal dependency error occurred, contact support.", innerException) { }
        public FlightDealDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
