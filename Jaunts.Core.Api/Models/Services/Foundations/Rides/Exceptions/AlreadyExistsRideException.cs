// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class AlreadyExistsRideException : Xeption
    {
        public AlreadyExistsRideException(Exception innerException)
            : base(message: "Ride with the same id already exists.", innerException) { }
        public AlreadyExistsRideException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
