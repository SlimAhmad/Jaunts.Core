// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class NullRideException : Xeption
    {
        public NullRideException() : base(message: "The Ride is null.") { }
        public NullRideException(string message) : base(message) { }
    }
}
