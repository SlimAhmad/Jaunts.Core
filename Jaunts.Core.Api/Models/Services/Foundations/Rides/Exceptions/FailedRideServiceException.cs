// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class FailedRideServiceException : Xeption
    {
        public FailedRideServiceException(Exception innerException)
            : base(message: "Failed Ride service error occurred, contact support",
                  innerException)
        { }
        public FailedRideServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
