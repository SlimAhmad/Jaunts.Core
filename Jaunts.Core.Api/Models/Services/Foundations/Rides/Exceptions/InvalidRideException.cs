// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class InvalidRideException : Xeption
    {
        public InvalidRideException()
            : base(message: "Invalid Ride. Please fix the errors and try again.")
        { }

        public InvalidRideException(string parameterName, object parameterValue)
            : base(message: $"Invalid student, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidRideException(string message)
            : base(message)
        { }
    }
}