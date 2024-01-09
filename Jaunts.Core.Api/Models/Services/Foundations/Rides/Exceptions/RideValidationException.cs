// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class RideValidationException : Xeption
    {
        public RideValidationException(Xeption innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public RideValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}