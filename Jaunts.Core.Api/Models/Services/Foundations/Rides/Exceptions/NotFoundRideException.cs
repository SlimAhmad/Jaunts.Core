// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class NotFoundRideException : Xeption
    {
        public NotFoundRideException(Guid RideId)
            : base(message: $"Couldn't find Ride with id: {RideId}.") { }
        public NotFoundRideException(string message)
            : base(message) { }
    }
}
