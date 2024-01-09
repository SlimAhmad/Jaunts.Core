// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class DriverDependencyValidationException : Xeption
    {
        public DriverDependencyValidationException(Xeption innerException)
            : base(message: "Driver dependency validation error occurred, fix the errors.", innerException) { }
        public DriverDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
