// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class DriverServiceException : Xeption
    {
        public DriverServiceException(Xeption innerException)
            : base(message: "Driver service error occurred, contact support.", innerException) { }
        public DriverServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}