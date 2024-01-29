// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class DriverValidationException : Xeption
    {
        public DriverValidationException(Xeption innerException)
            : base(message: "Driver validation error occurred, please try again.", innerException) { }
        public DriverValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}