// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class RideServiceException : Xeption
    {
        public RideServiceException(Xeption innerException)
            : base(message: "Ride service error occurred, contact support.", innerException) { }
        public RideServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}