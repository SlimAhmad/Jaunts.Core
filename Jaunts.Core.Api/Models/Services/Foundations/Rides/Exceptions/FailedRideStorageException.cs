// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class FailedRideStorageException : Xeption
    {
        public FailedRideStorageException(Exception innerException)
            : base(message: "Failed Ride storage error occurred, please contact support.", innerException)
        { }
        public FailedRideStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
