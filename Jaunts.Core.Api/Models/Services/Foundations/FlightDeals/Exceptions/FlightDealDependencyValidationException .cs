// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions
{
    public class FlightDealDependencyValidationException : Xeption
    {
        public FlightDealDependencyValidationException(Xeption innerException)
            : base(message: "FlightDeal dependency validation error occurred, fix the errors.", innerException) { }
        public FlightDealDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
