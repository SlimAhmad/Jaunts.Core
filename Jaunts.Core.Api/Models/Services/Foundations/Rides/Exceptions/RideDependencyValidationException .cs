// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class RideDependencyValidationException : Xeption
    {
        public RideDependencyValidationException(Xeption innerException)
            : base(message: "Ride dependency validation error occurred, fix the errors.", innerException) { }
        public RideDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
